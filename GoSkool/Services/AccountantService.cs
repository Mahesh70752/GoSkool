using GoSkool.Data;
using GoSkool.DTO;
using GoSkool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class AccountantService : IAccountantService
    {
        private readonly ApplicationDbContext _context;

        public AccountantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void FillAccountantHomeDetails(int AccountantId, AccountantHomeDTO accountantHomeDTO)
        {
            var Accountant = _context.Accountant.Find(AccountantId);
            accountantHomeDTO.Name = Accountant.FirstName+ ", " + Accountant.LastName;
            for (int i = 1; i <= 10; i++)
            {
                accountantHomeDTO.Classes.Add(new List<Tuple<int, string>>());
            }
            var AllClasses = _context.Classes.Include(x=>x.Standard).Include(x=>x.Section).Where(x => x.Id != 82).ToList();
            foreach(var Class in AllClasses)
            {
                int standard = Class.Standard.ClassNumber;
                accountantHomeDTO.Classes[standard - 1].Add(new Tuple<int,string>(Class.Id, Class.Standard.ClassNumber.ToString() + Class.Section.Name) );
            }
        }

        public int GetCurrentAccountantId(IdentityUser user)
        {
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((user).Id);
            var AccountantId = curUser.UserId;
            return AccountantId;
        }

        public void GetClassDetails(int ClassId, ClassDetailsDTO classDetailsDTO)
        {

            if (classDetailsDTO.SortParam == "" || classDetailsDTO.SortParam==null)
            {
                classDetailsDTO.NameSortParam = "NameAsc";
                classDetailsDTO.FeeSortParam = "FeeDesc";
            }
            classDetailsDTO.ClassId = ClassId;
            var Class = _context.Classes.Include(x=>x.Section).Include(x=>x.Standard).Where(x=>x.Id==ClassId).SingleOrDefault();
            classDetailsDTO.Name = Class.Standard.ClassNumber.ToString()+Class.Section.Name;
            var ClassStudents = _context.Students.Include(x => x.Class).Where(x => x.Class.Id == ClassId).ToList();
            classDetailsDTO.StudentsCount = ClassStudents.Count;
            foreach(var Student in ClassStudents)
            {
                classDetailsDTO.StudentFeeDetials.Add(new Tuple<string, int>(Student.FirstName + ", " + Student.LastName, 45000));
            }
            switch (classDetailsDTO.SortParam)
            {
                case "NameAsc":
                    classDetailsDTO.StudentFeeDetials = classDetailsDTO.StudentFeeDetials.OrderBy(x => x.Item1).ToList();
                    classDetailsDTO.NameSortParam = "NameDesc";
                    break;
                case "NameDesc":
                    classDetailsDTO.StudentFeeDetials = classDetailsDTO.StudentFeeDetials.OrderByDescending(x => x.Item1).ToList();
                    classDetailsDTO.NameSortParam = "NameAsc";
                    break;
                case "FeeDesc":
                    classDetailsDTO.StudentFeeDetials = classDetailsDTO.StudentFeeDetials.OrderByDescending(x => x.Item2).ToList();
                    classDetailsDTO.FeeSortParam = "FeeAsc";
                    break;
                case "FeeAsc":
                    classDetailsDTO.StudentFeeDetials = classDetailsDTO.StudentFeeDetials.OrderBy(x => x.Item2).ToList();
                    classDetailsDTO.FeeSortParam = "FeeDesc";
                    break;
                default:
                    break;
            }
        }
    }
}
