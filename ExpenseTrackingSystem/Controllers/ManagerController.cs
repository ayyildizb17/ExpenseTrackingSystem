using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Services;

namespace ExpenseTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ManagerController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("expenses")]
        public IActionResult GetExpenses()
        {
            // Get expenses for the manager's employees
            // Replace with your actual logic to fetch expenses
            var managerId = User.FindFirst("id")?.Value;
            var expenses = _expenseService.GetExpensesForManager(managerId);

            return Ok(expenses);
        }

        [HttpPost("approveExpense/{expenseId}")]
        public IActionResult ApproveExpense(int expenseId)
        {
            // Manager approves the expense
            // Replace with your actual logic to approve the expense
            var managerId = User.FindFirst("id")?.Value;
            var result = _expenseService.ApproveExpense(expenseId,managerId);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense approved successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        [HttpPost("rejectExpense/{expenseId}")]
        public IActionResult RejectExpense(int expenseId, [FromBody] RejectionReasonDto rejectionReasonDto)
        {
         
            var managerId = User.FindFirst("id")?.Value;
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
