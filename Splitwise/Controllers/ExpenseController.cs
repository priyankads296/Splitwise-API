using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Splitwise.Data;
using Splitwise.Migrations;
using Splitwise.Models;
using Splitwise.Services;
using System.Linq;
using System.Text;

namespace Splitwise.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        public ApplicationDbContext _dbContext;
      
        private IExpenseService _expenseService;

        public ExpenseController(ApplicationDbContext dbContext,IExpenseService expenseService)
        {
            _dbContext = dbContext;
          
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            var response=await _expenseService.GetAllExpenses();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            if (id == 0)
                return BadRequest("Enter valid id.");
            var response =await _expenseService.GetExpenseById(id);
            return Ok(response);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetExpenseByName(string name)
        {
            var response =await _expenseService.GetExpenseByName(name);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetExpenseByGroup(string groupName)
        {
           var response=await _expenseService.GetExpenseByGroup(groupName);
           return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetExpenseByGroupId(int groupId)
        {
            var response = await _expenseService.GetExpenseByGroupId(groupId);
            if (groupId == 0)
                return BadRequest(response.Message);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense, [FromQuery] int[] selectedUsersId, [FromQuery] int userPaidId)
        {
           var response=await _expenseService.AddExpense(expense, selectedUsersId, userPaidId);
           return Ok(response);

        }

        [HttpPut]
        public async Task<IActionResult> EditExpenseDetails(int id,decimal amount, string description)
        {
            if (id == 0)
            {
                return BadRequest("Please provide valid expense id to update");
            }
            var response=await _expenseService.EditExpenseDetails(id,amount, description);
            return Ok(response);


        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditExpense(int id, Expense expense)
        {

            if (expense == null || id != expense.ExpenseId)
            {
                return BadRequest("Please provide valid expense to update");
            }
            var response=await _expenseService.EditExpense(id,expense);
            return Ok(response);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (id == 0)
                return BadRequest("Enter valid id.");
            var response=await _expenseService.DeleteExpense(id);
            return Ok(response);
        }

        //void settleExpense()

        //public async Task<IActionResult> AddExpense(ExpenseDetail expenseDetail, List<User> users = null, string groupName = null)   //=null allows to make parameter optional
        //public async Task<IActionResult> AddExpense(ExpenseDetail expenseDetail, List<User> users = null, string groupName = null)
        //{
        //    if (groupName != null)
        //    {
        //        add expense to group members
        //        Group existingGroupInDb = await _dbContext.Groups.Include(u => u.Users).FirstOrDefaultAsync(e => e.Title.ToLower() == groupName.ToLower());
        //        if (existingGroupInDb == null)
        //        {
        //            return NotFound("Group doesn't exist");
        //        }
        //        var usersInGroup = existingGroupInDb.Users;
        //        Expense expense = new Expense();
        //        expense.UsersInvolved = usersInGroup;
        //        expense.ExpenseDetails = expenseDetail;
        //        await _dbContext.Expenses.AddAsync(expense);
        //        await _dbContext.SaveChangesAsync();
        //        return Ok(expense);


        //    }
        //    else if (users != null)
        //    {
        //        add expense to selected users
        //        Expense expense = new Expense();
        //        expense.UsersInvolved = users;
        //        expense.ExpenseDetails = expenseDetail;
        //        await _dbContext.Expenses.AddAsync(expense);
        //        await _dbContext.SaveChangesAsync();
        //        return Ok(expense);
        //    }
        //    else
        //    {
        //        no users or groupName provided
        //        return BadRequest("Please provide group name or users");
        //    }

        //}

        //[HttpPost]
        //public async Task<IActionResult> AddExpense(string userName, string expenseTitle, ExpenseDetail expenseDetail, string groupName)
        //{
        //    Group existingGroupInDb = await _dbContext.Groups.FirstOrDefaultAsync(e => e.Title.ToLower() == groupName.ToLower());
        //    User existingUserInDb = await _dbContext.Users.FirstOrDefaultAsync(e => e.Name.ToLower() == userName.ToLower());
        //    Expense existingExpense = _dbContext.Expenses.Include(e => e.ExpenseDetails)
        //                                .FirstOrDefault(e => e.Title == groupName && existingUserInDb.Name.ToLower() == userName.ToLower());
        //    if (existingExpense == null)
        //    {
        //        existingExpense = new Expense();
        //        existingExpense.GroupId = existingGroupInDb.GroupId;
        //        existingExpense.UserId = existingUserInDb.UserId;
        //        existingExpense.Title = groupName;
        //        existingExpense.ExpenseDetails = expenseDetail;
        //        _dbContext.Expenses.Add(existingExpense);
        //    }
        //    else
        //    {
        //        if (existingExpense.ExpenseDetails == null)
        //        {
        //            existingExpense.ExpenseDetails = expenseDetail;
        //        }
        //        else if (existingExpense.Title.ToLower() == expenseTitle.ToLower())
        //        {
        //            existingExpense.ExpenseDetails.Amount += expenseDetail.Amount;
        //        }
        //        else
        //        {
        //            var newExpenseDetail = new ExpenseDetail();
        //            newExpenseDetail.Id = expenseDetail.Id;
        //            newExpenseDetail.Amount = expenseDetail.Amount;
        //            newExpenseDetail.ExpenseId = expenseDetail.ExpenseId;
        //            newExpenseDetail.Description = expenseDetail.Description;

        //            await _dbContext.SaveChangesAsync();
        //            return Ok(existingExpense);

        //        }

        //    }

        //    await _dbContext.SaveChangesAsync();
        //    return Ok(existingExpense);
        //}
        //[HttpPost("AddBalance")]
        //public async Task<IActionResult> AddBalance(int userId, int amount)
        //{
        //    var balances = await _dbContext.Balances.FirstOrDefaultAsync(b => b.UsersId == userId);
        //    Balance balance = new Balance();
        //    balance.UserId = userId;
        //    if (balances == null)
        //    {
                
        //        balance.UserId = userId;
        //        balance.Amount = amount;
        //        _context.Balances.Add(balance);
        //        _context.SaveChanges();
        //        return Ok(balance);
        //    }

        //    if (balances.UsersId == userId)
        //    {
        //        balance.Amount = balance.Amount + amount;
        //        _context.Balances.Add(balance);
        //        _context.SaveChanges();
        //        return Ok(balance);
        //    }
        //    else
        //    {
        //        return Ok("user ID not found");
        //    }


        //    return Ok(balance);
        //}
    }
   
}
