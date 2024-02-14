using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Services;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  //  [Authorize(Policy = "ManagerPolicy")]

    public class ManagerController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomUserRole> _roleManager;
        public ManagerController(IExpenseService expenseService, UserManager<CustomUser> userManager, RoleManager<CustomUserRole> roleManager)
        {
            _expenseService = expenseService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("expenses")]
        public IActionResult GetExpenses()
        {

            // var managerId = User.FindFirst("id")?.Value;
            var managerId = "0ab5a82e-8204-499c-86aa-d41726427fe7";
            var expenses = _expenseService.GetExpensesForManager(managerId);

            return Ok(expenses);
        }

        [HttpPost("approveExpense/{expenseId}")]
        public IActionResult ApproveExpense(int expenseId)
        {

            // var managerId = User.FindFirst("id")?.Value;
            var managerId = "0ab5a82e-8204-499c-86aa-d41726427fe7";
            var result = _expenseService.ApproveExpense(expenseId,managerId);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense approved successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        [HttpPost("rejectExpense/{expenseId}")]
        public IActionResult RejectExpense(int expenseId, [FromBody] RejectionReasonDto rejectionReasonDto)
        {

            // var managerId = User.FindFirst("id")?.Value;
            var managerId = "0ab5a82e-8204-499c-86aa-d41726427fe7";
            var result = _expenseService.RejectExpense(expenseId, managerId, rejectionReasonDto.Reason);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense rejected successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        public class RejectionReasonDto
        {
            public string Reason { get; set; }
        }
    }
}
