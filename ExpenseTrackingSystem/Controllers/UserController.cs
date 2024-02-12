using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Services;

namespace ExpenseTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public EmployeeController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("expenses")]
        public IActionResult GetExpenses()
        {
            // Get expenses for the current employee
            // Replace with your actual logic to fetch expenses
            var employeeId = User.FindFirst("id")?.Value;
            var expenses = _expenseService.GetExpensesForEmployee(employeeId);

            return Ok(expenses);
        }

        [HttpPost("createExpense")]
        public IActionResult CreateExpense([FromBody] ExpenseCreateDto expenseCreateDto)
        {
            // Employee creates a new expense
            // Replace with your actual logic to create an expense
            var employeeId = User.FindFirst("id")?.Value;
            var result = _expenseService.CreateExpense(employeeId, expenseCreateDto);

            if (result.SuccessSituation.GetType == ServiceResult.Success)
                return Ok(new { Message = "Expense created successfully." });
            else
                return BadRequest(new { Message = result.Message });
        }

        [HttpDelete("cancelExpense/{expenseId}")]
        public IActionResult CancelExpense(int expenseId)
        {
            // Employee cancels their own expense
            // Replace with your actual logic to cancel the expense
            var employeeId = User.FindFirst("id")?.Value;
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
