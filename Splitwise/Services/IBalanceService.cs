using Microsoft.AspNetCore.Mvc;
using Splitwise.Models;

namespace Splitwise.Services
{
    public interface IBalanceService
    {
        public Task<Response> GetAllBalances();

        public Task<Response> GetBalanceByUser(int userId);
        public Task<Response> AddBalance(int userId, decimal amount);
        public Task<Response> AddBalanceForUserInvolved(int userId, decimal IndividualAmount);
        public Task<Response> AddBalanceForUserPaid(int userId, decimal amount);
        public Task<Response> Calculation(int expId);
        public Task<Response> DeleteBalance(int balId);
        public Task<Response> OptimizeTransactions(int groupId);
    }
}
