using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Entities;
using ExpenseTrackingSystem.Services;

namespace ExpenseTrackingSystem.Repositories
{
    public interface IExpenseRepository
    {
        List<Expense> GetExpensesForEmployee(string employeeId);
        List<Expense> GetExpensesForManager(string managerId);
        Expense GetExpenseById(int expense);
        Employee GetEmployeeById(string employeeId);
        void CreateExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void ApproveExpense(Expense formerExpense);
        void RejectExpense(Expense formerExpense, string rejectionReason);
        void CancelExpense(int expenseId);

    }
}
