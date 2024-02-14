using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Services;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = "EmployeePolicy")]
    public class EmployeeController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomUserRole> _roleManager;

        public EmployeeController(IExpenseService expenseService, UserManager<CustomUser> userManager, RoleManager<CustomUserRole> roleManager)
        {
            _expenseService = expenseService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("expenses")]
        public IActionResult GetExpenses()
        {


            //          var employeeId = User.FindFirst("id")?.Value;
            var employeeId = "2c08825a-02c2-4615-a4da-c4563b284d1f";
            var expenses = _expenseService.GetExpensesForEmployee(employeeId);

            return Ok(expenses);
        }

        [HttpPost("createExpense")]
        public IActionResult CreateExpense([FromBody] ExpenseCreateDto expenseCreateDto)
        {

            //          var employeeId = User.FindFirst("id")?.Value;
            var employeeId = "2c08825a-02c2-4615-a4da-c4563b284d1f";
            var result = _expenseService.CreateExpense(employeeId, expenseCreateDto);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense created successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        [HttpDelete("cancelExpense/{expenseId}")]
        public IActionResult CancelExpense(int expenseId)
        {
            //          var employeeId = User.FindFirst("id")?.Value;
            var employeeId = "2c08825a-02c2-4615-a4da-c4563b284d1f";
            var result = _expenseService.CancelExpense(employeeId, expenseId);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense canceled successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        public class ExpenseCreateDto
        {
            public double Amount { get; set; }
            public string Description { get; set; }
            public string City { get; set; }
            public DateTime ExpenseDate { get; set; }
        }
    }
}
