using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackingSystem.Entities.Models
{
    public class Expense
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("ManagerId")]
        public string ManagerId { get; set; }
        public Manager Manager { get; set; }

        public string Status { get; set; }
        public string RejectionReason { get; set; }



    }
}
