using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoSkool.Models
{
    public class ClassEntity
    {
        public int Id { get; set; }
        public StandardEntity Standard { get; set; }
        public SectionEntity Section { get; set; }
        [ValidateNever]
        public ICollection<SubjectEntity> Subjects { get; set; }
    }
}
