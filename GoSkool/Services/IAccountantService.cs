using GoSkool.DTO;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Services
{
    public interface IAccountantService
    {
        void FillAccountantHomeDetails(int AccountantId,AccountantHomeDTO accountantHomeDTO);
        int GetCurrentAccountantId(IdentityUser user);

        void GetClassDetails(int ClassId,ClassDetailsDTO classDetailsDTO);
    }
}
