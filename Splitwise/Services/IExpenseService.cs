using Microsoft.AspNetCore.Mvc;
using Splitwise.Models;

namespace Splitwise.Services
{
    public interface IExpenseService
    {
        public Task<Response> GetAllExpenses();
        //public Task<IActionResult> GetExpenseById(int id);
        public Task<Response> GetExpenseByGroup(string groupName);
        public Task<Response> GetExpenseByGroupId(int groupId);
        public Task<Response> GetExpenseById(int id);
        public Task<Response> GetExpenseByName(string name);
        public Task<Response> AddExpense(Expense expense, int[] selectedUsersId, int userPaidId);
        public Task<Response> EditExpenseDetails(int id, decimal amount, string description);
        public Task<Response> EditExpense(int id, Expense expense);
        public Task<Response> DeleteExpense(int id);

    }
}
