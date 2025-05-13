using GoSkool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoSkool.Views.Class
{
    public class IndexModel
    {
        public ClassEntity Class { get; set; }
        public IEnumerable<StudentEntity> Students { get; set; }
        public string SubjectName { get; set; }
        public IEnumerable<SubjectEntity> Subjects { get; set; }

        public IEnumerable<SelectListItem> teachers { get; set; }

        public string SelectedTeacher { get; set; }


    }
}
