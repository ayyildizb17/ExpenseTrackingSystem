using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingSystem.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly CustomContext _context;

        public ExpenseRepository(CustomContext context)
        {
            _context = context;
        }

        // Get all expenses for a specific employee
        public List<Expense> GetExpensesForEmployee(string employeeId)
        {
            return _context.Expenses
                .Where(e => e.EmployeeId == employeeId)
                .ToList();
        }

        // Get all expenses for a specific manager's employees
        public List<Expense> GetExpensesForManager(string managerId)
        {
            return _context.Expenses
                .Where(e => e.ManagerId == managerId)
                .ToList();
        }


        public Expense GetExpenseById(int expenseId)
        {
            return _context.Expenses.Find(expenseId);
        }

        public Employee GetEmployeeById(string employeeId)
        {
            return _context.Employees.Find(employeeId);
        }


        public void CreateExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        // Update an existing expense
        public void UpdateExpense(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ApproveExpense(Expense former_expense)
        {
            Expense expense = former_expense;
            expense.Status = "Approved";
            _context.Entry(expense).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RejectExpense(Expense formerExpense, string rejectionReason)
        {
            Expense expense = formerExpense;
            expense.Status = "Rejected";
            expense.RejectionReason = rejectionReason;
            _context.Entry(expense).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public void CancelExpense(int expenseId)
        {
            var expense = _context.Expenses.Find(expenseId);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
        }
    }
}

