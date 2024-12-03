using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Splitwise.Data;
using Splitwise.Models;
using Splitwise.Services;

namespace Splitwise.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ApplicationDbContext _dbContext;
        public BalanceController(ApplicationDbContext dbContext,IBalanceService balanceService) 
        {
            _dbContext = dbContext;
            _balanceService = balanceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBalances()
        {
            var res = await _balanceService.GetAllBalances();
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetBalanceByUser(int userId)
        {
            if (userId <= 0) { return BadRequest("Enter valid id..."); }
            var response = await _balanceService.GetBalanceByUser(userId);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddBalance(int userId, decimal amount)
        {
            if (userId == 0) { return BadRequest("Enter valid id..."); }
            var response=await _balanceService.AddBalance(userId, amount);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBalanceForUserInvolved(int userId, decimal IndividualAmount)
        {
            if(userId == 0) { return BadRequest("Enter valid id..."); }
            var response=await _balanceService.AddBalanceForUserInvolved(userId, IndividualAmount);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBalanceForUserPaid(int userId, decimal amount)
        {
            if(userId == 0) { return BadRequest("Enter valid id..."); }
            var response=await _balanceService.AddBalanceForUserPaid(userId, amount);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> OptimizeTransactions(int groupId)
        {
            if (groupId == 0) { return BadRequest("Enter valid id..."); }
            var response=await _balanceService.OptimizeTransactions(groupId);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBalance(int balanceId)
        {
            if(balanceId == 0) { return BadRequest("Balance not found..."); }
            var response=await _balanceService.DeleteBalance(balanceId);
            return Ok(response);
        }

    }
}
