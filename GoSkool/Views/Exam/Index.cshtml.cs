using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoSkool.Views.Exam
{
    public class ExamModel
    {
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public string ClassId { get; set; }
        public int subjectId {  get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> classList { get; set; }
    }
}
