using System.Threading.Tasks;
using GoSkool.Data;
using GoSkool.Models;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.ViewModels.Admin
{
    public class AdminHomeModel
    {
        public string selection { get; set; }
        public IEnumerable<ClassEntity> classes { get; set; }
        public IEnumerable<TeacherEntity> teachers { get; set; }
        public IEnumerable<AdminEntity> admins { get; set; }

        public readonly ApplicationDbContext _context;

        public AdminHomeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Tuple<StandardEntity,List<ClassEntity>>> AllClasses { get; set; }
        public async Task FillAllClasses()
        {
            AllClasses = new List<Tuple<StandardEntity, List<ClassEntity>>>();
            var standards = await _context.Standard.ToListAsync();
            standards =standards.OrderBy(x=>x.ClassNumber).ToList();
            foreach (var standard in standards)
            {
                List<ClassEntity> sClasses = new List<ClassEntity>();
                foreach (var cls in classes) {
                    if(cls.Standard.ClassNumber == standard.ClassNumber)sClasses.Add(cls);
                }
                AllClasses.Add(new Tuple<StandardEntity, List<ClassEntity>>(standard, sClasses));
            }
        }
    }
}
