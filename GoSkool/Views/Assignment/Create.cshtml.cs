using GoSkool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoSkool.Views.Assignment
{
    public class AssignmentCreationModel
    {
        public IEnumerable<SelectListItem> ClassList { get; set; }
        public AssignmentEntity Assignment { get; set; }
        public string classId { get; set; }
    }
}
