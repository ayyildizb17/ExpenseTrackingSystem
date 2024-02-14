using System;
using System.Collections.Generic;
using ExpenseTrackingSystem.Controllers;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Services.Model;

namespace ExpenseTrackingSystem.Services
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetExpensesForEmployee(string employeeId);
        ServiceResult CreateExpense(string employeeId, EmployeeController.ExpenseCreateDto expenseCreateDto);
        ServiceResult CancelExpense(string employeeId, int expenseId);
        IEnumerable<Expense> GetExpensesForManager(string managerId);
        ServiceResult ApproveExpense(int expenseId, string managerId);
        ServiceResult RejectExpense(int expenseId, string managerId, string rejectionReason);
    }

  
}
