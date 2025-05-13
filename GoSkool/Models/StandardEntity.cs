using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoSkool.Models
{
    public class StandardEntity
    {
        public int Id { get; set; }
        [Range(1,10)]
        public int ClassNumber {  get; set; }
        
    }
}
