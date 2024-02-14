using ExpenseTrackingSystem.Controllers;
using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Repositories;
using ExpenseTrackingSystem.Services.Model;

namespace ExpenseTrackingSystem.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public IEnumerable<Expense> GetExpensesForEmployee(string employeeId)
        {

            return _expenseRepository.GetExpensesForEmployee(employeeId);
        }

        public ServiceResult CreateExpense(string employeeId, EmployeeController.ExpenseCreateDto expenseCreateDto)
        {

            var expense = new Expense();
            expense.EmployeeId = employeeId;
            expense.Status = "Waiting for approval.";
            expense.RejectionReason = "-";
            expense.Cost = expenseCreateDto.Amount;
            expense.ExpenseDate = DateTime.Now;
            expense.City = expenseCreateDto.City;
            expense.Description = expenseCreateDto.Description;
            Employee employee = _expenseRepository.GetEmployeeById(employeeId);
            expense.ManagerId = employee.ManagerId;

            _expenseRepository.CreateExpense(expense);

            return ServiceResult.Success("Expense created successfully.");
        }

        public ServiceResult CancelExpense(string employeeId, int expenseId)
        {

            var expense = _expenseRepository.GetExpenseById(expenseId);

            if (expense == null || expense.EmployeeId != employeeId)
            {

                return ServiceResult.Error("Expense not found or unauthorized.");
            }

            _expenseRepository.CancelExpense(expense.Id);

            return ServiceResult.Success("Expense canceled successfully.");
        }


        public IEnumerable<Expense> GetExpensesForManager(string managerId)
        {

            return _expenseRepository.GetExpensesForManager(managerId);
        }

        public ServiceResult ApproveExpense(int expenseId, string managerId)
        {

            var expense = _expenseRepository.GetExpenseById(expenseId);

            if (expense == null || expense.ManagerId != managerId)
            {

                return ServiceResult.Error("Expense not found or unauthorized.");
            }

            _expenseRepository.ApproveExpense(expense);


            return ServiceResult.Success("Expense approved successfully.");
        }

        public ServiceResult RejectExpense(int expenseId, string managerId, string rejectionReason)
        {

            var expense = _expenseRepository.GetExpenseById(expenseId);

            if (expense == null || expense.ManagerId != managerId)
            {

                return ServiceResult.Error("Expense not found or unauthorized.");
            }


            _expenseRepository.RejectExpense(expense, rejectionReason);


            return ServiceResult.Success("Expense rejected successfully.");
        }
    }
}
