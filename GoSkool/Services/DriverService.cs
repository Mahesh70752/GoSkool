using System.Configuration;
using System.Dynamic;
using System.Security.Policy;
using Azure;
using GoSkool.Data;
using GoSkool.DTO;
using GoSkool.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace GoSkool.Services
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        //GoogleMatrix Api Response -> Object
        public class Response
        {
            public string Status { get; set; }

            [JsonProperty(PropertyName = "origin_addresses")]
            public string[] OriginAddresses { get; set; }

            [JsonProperty(PropertyName = "destination_addresses")]
            public string[] DestinationAddresses { get; set; }

            public Row[] Rows { get; set; }

            public class Data
            {
                public int Value { get; set; }
                public string Text { get; set; }
            }

            public class Element
            {
                public string Status { get; set; }
                public Data Duration { get; set; }
                public Data Distance { get; set; }
            }

            public class Row
            {
                public Element[] Elements { get; set; }
            }
        }

        public DriverService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void FillDriverDetails(IdentityUser user, DriverDTO driver)
        {
            driver.Id = ((GoSkoolUser)_context.Users.Find(user.Id)).UserId;
            var curDriver = _context.Driver.Find(driver.Id);
            driver.BusNumber = curDriver.BusNumber;
            driver.Name = curDriver.FirstName + ", " + curDriver.LastName;
            var busLocations = _context.Location.Where(bus => bus.BusNumber == driver.BusNumber).SingleOrDefault();
            if(busLocations == null)
            {
                driver.lats = new List<string>();
                driver.lngs = new List<string>();
                return;
            }
            driver.lats = busLocations.lats;
            driver.lngs = busLocations.lngs;
        }

        public async void SaveLocation(LocationDTO location)
        {
            Console.WriteLine(location.latitude + " " + location.longitude);
            var LocationDetails = _context.Location.Where(loc => loc.BusNumber == location.BusNumber).SingleOrDefault();
            if(LocationDetails == null)
            {
                LocationDetails = new LocationEntity() { BusNumber = location.BusNumber };
                LocationDetails.lats = new List<string>();
                LocationDetails.lngs = new List<string>(); 
                LocationDetails.lats.Add(location.latitude);
                LocationDetails.lngs.Add(location.longitude);
                _context.Location.Add(LocationDetails);
                _context.SaveChangesAsync().Wait();
                return;
            }

            var lastLat = double.Parse(LocationDetails.lats.LastOrDefault());
            var lastLng = double.Parse(LocationDetails.lngs.LastOrDefault());

            var curLat = double.Parse(location.latitude);
            var curLng = double.Parse(location.longitude);

            var Url = _configuration.GetValue<string>("GoogleMaps:DistanceMatrix");

            var origins = lastLat +","+ lastLng;
            var destinations = curLat +","+ curLng;

            var Key = _configuration.GetValue<string>("GoogleMaps:APIKey");

            var requestUrl = $"{Url}?origins={origins}&destinations={destinations}&key={Key}";

            var distance = 0;
            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<Response>(content);
                    distance = res.Rows[0].Elements[0].Distance.Value;
                    Console.WriteLine("=============================================");
                    Console.WriteLine(distance);
                    Console.WriteLine("=============================================");
                    

                }
            }
            if (distance > 500)
            {
                Console.WriteLine("Added to DB");
                LocationDetails.lats.Add(location.latitude);
                LocationDetails.lngs.Add(location.longitude);
                _context.Location.Update(LocationDetails);
                _context.SaveChanges();
            }

        }
    }
}
