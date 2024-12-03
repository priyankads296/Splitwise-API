using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Splitwise.Controllers;
using Splitwise.Data;
using Splitwise.Migrations;
using Splitwise.Models;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Splitwise.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _dbContext;
        public IBalanceService _balanceService;

        public ExpenseService(ApplicationDbContext dbContext,IBalanceService balanceService)
        {
            _dbContext = dbContext;
            _balanceService = balanceService;

        }
        public async Task<Response> GetAllExpenses()
        {
            Response res = new Response();
            res.Status = true;
            var expenses = await _dbContext.Expenses.Include(e => e.ExpenseDetails).Include(e => e.UsersInvolved).ToListAsync();
            if (expenses == null) { res.Message = "Error fetching data...."; return res; }
            foreach (var expense in expenses)
            {
                if (expense == null || expense.ExpenseDetails == null || expense.UsersInvolved == null) { continue; }
            }
            res.Data = expenses;
            res.Message = "Expenses fetched Successfully...";
            return res;
        }
        public async Task<Response> GetExpenseById(int id)
        {
            Response res = new Response();
            res.Status = true;
            var expense = await _dbContext.Expenses.Include(e => e.ExpenseDetails).Include(e => e.UsersInvolved).FirstOrDefaultAsync(e => e.ExpenseId == id);
            if (expense == null) { res.Message = "Expense not found."; return res; }
            res.Data = expense;
            return res;

        }
        public async Task<Response> GetExpenseByName(string name)
        {
            Response res = new Response();
            res.Status = true;
            var expenses = await _dbContext.Expenses.Include(e => e.UsersInvolved)
                            .Include(e => e.ExpenseDetails)
                            .Where(u => u.UsersInvolved.Any(n => n.Name.ToLower() == name.ToLower())).ToListAsync();
            if (expenses == null) { res.Message = "Expense not found."; return res; }
            res.Data = expenses;
            return res;
        }
        public async Task<Response> GetExpenseByGroup(string groupName)
        {
            Response res = new Response();
            res.Status = true;
            var expenses = await _dbContext.Expenses.Include(e => e.UsersInvolved)
                            .Include(e => e.ExpenseDetails)
                            .Where(e => e.GroupName.ToLower() == groupName.ToLower()).ToListAsync();
            if (expenses == null) { res.Message = "Expense not found."; return res; }
            res.Data = expenses;
            return res;
        }
        public async Task<Response> GetExpenseByGroupId(int groupId)
        {
            Response res = new Response();
            res.Status = true;
            var expenses = await _dbContext.Expenses.Include(e => e.UsersInvolved)
                            .Include(e => e.ExpenseDetails)
                            .Where(e => e.GroupId == groupId).ToListAsync();
            if (expenses == null) { res.Message = "Expense not found."; return res; }
            res.Data = expenses;
            return res;
        }
        public async Task<Response> AddExpense([FromBody] Expense expense, [FromQuery] int[] selectedUsersId, [FromQuery] int userPaidId)
        {
            Response response = new Response();
            response.Status = true;

            var groupId = expense.GroupId;
            Group group = await _dbContext.Groups.Include(u => u.Users).Include(g => g.GroupDetails).FirstOrDefaultAsync(e => e.GroupId == groupId);

            if (group != null && expense.GroupId != 0)
            {
                //if group exist it will divide expense among all users
                var usersFromGroup = group.Users.ToList();
                if (usersFromGroup == null)
                {
                    response.Message = "No user found in the group.Please add individual users.";
                    return response;
                }

                expense.UsersInvolved = usersFromGroup.Where(u => selectedUsersId.Contains(u.UserId)).ToList();
                expense.UsersPaid = usersFromGroup.Where(u => u.UserId == userPaidId).ToList();


            }
            else
            {

                //for individual users
                var usersSelected = _dbContext.Users.Where(u => selectedUsersId.Contains(u.UserId)).ToList();
                var userPaid = _dbContext.Users.Where(u => u.UserId == userPaidId).ToList();

                expense.UsersInvolved = usersSelected;
                expense.UsersPaid = userPaid;

                //_dbContext.Expenses.AddAsync(expense);
                //await _dbContext.SaveChangesAsync();

            }

            await _dbContext.Expenses.AddAsync(expense);
            await _dbContext.SaveChangesAsync();

            var responseFromCalculationApi = _balanceService.Calculation(expense.ExpenseId);
            if (responseFromCalculationApi.Result.Status)
            {
                response.Message = "Expense Added Successfully. Balance updated";
                response.Data = expense;

            }
            else
            {
                response.Message = "Expense Added Successfully. Error in adding Balance to individual users.";
                response.Data = expense;
            }


            return response;

        }
        public async Task<Response> EditExpenseDetails(int id, decimal amount, string description)
        {
            Response res = new Response();
            res.Status = true;
            if (id == 0)
            {
                res.Message = "Enter valid expense id.";
                return res;
            }
            Expense existingExpenseInDb = await _dbContext.Expenses.Include(e => e.UsersInvolved).Include(e => e.ExpenseDetails).FirstOrDefaultAsync(i => i.ExpenseId == id);
            if (existingExpenseInDb == null)
            {
                res.Message = "Expense id not found";
                return res;
            }
                

            existingExpenseInDb.ExpenseDetails.Amount = amount;
            existingExpenseInDb.ExpenseDetails.Description = description;
            await _dbContext.SaveChangesAsync();
            res.Data = existingExpenseInDb;
            return res;


        }
        public async Task<Response> EditExpense(int id, Expense expense)
        {
            Response res = new Response();
            res.Status = true;
            if (expense == null || id != expense.ExpenseId)
            {
                res.Message = "Please provide valid expense to update";
                return res;
            }
            Expense existingExpenseInDb = await _dbContext.Expenses.Include(e => e.UsersInvolved).Include(e => e.ExpenseDetails).FirstOrDefaultAsync(i => i.ExpenseId == id);
            if (existingExpenseInDb == null)
            {
                res.Message = "Expense doesn't exist.";
                return res;
            }
            
            existingExpenseInDb.ExpenseDetails.Amount = expense.ExpenseDetails.Amount;
            existingExpenseInDb.ExpenseDetails.Description = expense.ExpenseDetails.Description;
            foreach (var user in expense.UsersInvolved)  
            {
                if (existingExpenseInDb.UsersInvolved.Any(u => u.Name.ToLower() == user.Name.ToLower()))
                {
                    continue;
                }
                existingExpenseInDb.UsersInvolved.Add(user);
            }

            ////_dbContext.Expenses.AddAsync(existingExpenseInDb);
            await _dbContext.SaveChangesAsync();
            res.Data = existingExpenseInDb;
            return res;

        }
        public async Task<Response> DeleteExpense(int id)
        {
            Response res = new Response();
            res.Status = true;
            var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.ExpenseId == id);
            if (expense == null)
            {
                res.Message = "Expense doesn't exist.Please provide valid expense to update";
                return res;
            }
            _dbContext.Expenses.Remove(expense);
            await _dbContext.SaveChangesAsync();
            res.Message = "Expense Id " + id + " Deleted Successfully.";
            return res;
        }

    }
}
