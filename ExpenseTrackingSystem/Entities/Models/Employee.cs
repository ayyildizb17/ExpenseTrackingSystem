using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackingSystem.Entities.Models
{
    public class Employee : CustomUser
    {


        public string ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Manager Manager { get; set; }

    
    public ICollection<Expense> Expenses { get; set; }
    }
}
