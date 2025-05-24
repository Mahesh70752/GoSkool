namespace GoSkool.Models
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public int BusNumber { get; set; }
        public List<string> lats { get; set; }
        public List<string> lngs { get; set; }
    }
}
