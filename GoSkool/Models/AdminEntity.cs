using System.ComponentModel.DataAnnotations.Schema;

namespace GoSkool.Models
{
    public class AdminEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}
