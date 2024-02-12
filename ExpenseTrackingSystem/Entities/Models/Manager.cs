using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackingSystem.Entities.Models
{
    public class Manager : CustomUser
    {
      
        public ICollection<Employee> Employees { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
