using Microsoft.AspNetCore.Identity;

namespace GoSkool.Models
{
    public class GoSkoolUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public int Age {  get; set; }

        public int UserId {  get; set; }
    }
}
