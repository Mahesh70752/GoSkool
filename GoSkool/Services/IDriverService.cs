using GoSkool.DTO;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Services
{
    public interface IDriverService
    {
        void SaveLocation(LocationDTO location);
        void FillDriverDetails(IdentityUser user,DriverDTO driver);
    }
}
