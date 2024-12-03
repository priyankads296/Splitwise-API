using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Models;

namespace Splitwise.Services
{
    public class BalanceService:IBalanceService
    {
        private readonly ApplicationDbContext _dbContext;
        
        public BalanceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public async Task<Response> GetAllBalances()
        {
            Response res = new Response();
            res.Status = true;

            var balances=await _dbContext.Balances.ToListAsync();
            if (balances == null)
            {
                res.Status = false;
                res.Message = "No data found";
                return res;
            }
            res.Data=balances;
            res.Message = "Balances fetched Successfully...";
            return res;
        }
        public async Task<Response> GetBalanceByUser(int userId)
        {
            Response res = new Response();
            res.Status = true;

            var balance = await _dbContext.Balances.FirstOrDefaultAsync(i=>i.UserId==userId);
            if (balance == null)
            {
                res.Status = false;
                res.Message = "No data found";
                return res;
            }
            res.Data = balance;
            res.Message = "Balance fetched Successfully...";
            return res;
        }

        public async Task<Response> AddBalance(int userId, decimal amount)
        {
            Response res = new Response();
            res.Status = true;

            var balance=await _dbContext.Balances.FirstOrDefaultAsync(b=>b.UserId==userId);
            if (balance == null)
            {
                balance = new Balance()
                {
                    UserId = userId,
                    Amount = -amount

                };

                await _dbContext.Balances.AddAsync(balance);
            }
            else
            {
                balance.Amount -= amount;

            }
            await _dbContext.SaveChangesAsync();
            res.Data = balance;
            res.Message = "Balance added Successfully";
            return res;

        }
        public async Task<Response> Calculation(int expId)
        {
            Response res = new Response();
            res.Status = true;

            Expense expense = await _dbContext.Expenses.Include(u => u.ExpenseDetails).Include(u => u.UsersInvolved).Include(u => u.UsersPaid).FirstOrDefaultAsync(e => e.ExpenseId == expId);
            if (expense == null)
            {
                res.Message = "Expense doesn't exist. Please enter valid expense.";
                res.Data = expense;
                return res;
            }
            var NumberOfUsers = expense.UsersInvolved.Count();
            if (NumberOfUsers <= 0)
            {
                res.Message = "No user found in the expense.";
                res.Data = expense;
                return res;

            }
            var Amount = expense.ExpenseDetails.Amount;

            var IndividualAmount = Amount / NumberOfUsers;

            //add balance of userPaid
            foreach (var user in expense.UsersPaid)
            {
                var addBalanceResponseForUserPaid = await AddBalanceForUserPaid(user.UserId, Amount);
                if (addBalanceResponseForUserPaid.Status)
                {
                    var balance = addBalanceResponseForUserPaid.Data;
                    res.Message = "Balance added successfully for user" + user.UserId;
                }
                else
                {
                    res.Status = false;
                    res.Message = "Error in adding Balance";
                }
            }

            //add balance of userInvolved
            foreach (var user in expense.UsersInvolved)
            {
                var addBalanceResponseForUserInvolved = await AddBalanceForUserInvolved(user.UserId, IndividualAmount);
                if (addBalanceResponseForUserInvolved.Status)
                {
                    var balance = addBalanceResponseForUserInvolved.Data;
                    res.Message = "Balance added successfully for user" + user.UserId;
                }
                else
                {
                    res.Status = false;
                    res.Message = "Error in adding Balance";
                }
            }
            res.Data = expense;
           // res.Message = "Expense added Successfully.";
            var transaction = OptimizeTransactions(expense.GroupId);
            res.Message = "Expense added Successfully and transaction updated...";
            return res;
        }
        public async Task<Response> AddBalanceForUserInvolved(int userId, decimal IndividualAmount)
        {
            Response res = new Response();
            res.Status = true;

            var balances = await _dbContext.Balances
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (balances == null)
            {
                balances = new Balance()
                {
                    UserId = userId,
                    Amount = -IndividualAmount

                };

                await _dbContext.Balances.AddAsync(balances);
            }
            else
            {
                balances.Amount -= IndividualAmount;

            }

            await _dbContext.SaveChangesAsync();
            res.Data = balances;
            res.Message = "Balance added Successfully";
            return res;
        }

        public async Task<Response> AddBalanceForUserPaid(int userId, decimal amount)
        {
            Response res = new Response();
            res.Status = true;

            var balances = await _dbContext.Balances.FirstOrDefaultAsync(b => b.UserId == userId);
            if (balances == null)
            {
                balances = new Balance()
                {
                    UserId = userId,
                    Amount = amount
                };
                await _dbContext.Balances.AddAsync(balances);
            }
            else
            {
                balances.Amount += amount;
            }
            await _dbContext.SaveChangesAsync();

            res.Data = balances;
            res.Message = "Balance added Successfully";
            return res;
        }


        public async Task<Response> OptimizeTransactions(int groupId)
        {
            Response res = new Response();

            Group group = await _dbContext.Groups.Include(e => e.Users).Include(e => e.GroupDetails)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
            var users = group.Users;

            var bal = _dbContext.Balances.ToList();

            List<Transaction> transactions = new List<Transaction>();

            Dictionary<int, decimal> Pos = new Dictionary<int, decimal>();
            Dictionary<int, decimal> Neg = new Dictionary<int, decimal>();

            foreach (var user in users)
            {
                var userBal = bal.FirstOrDefault(b => b.UserId == user.UserId);
                if (userBal != null)
                {
                    if (userBal.Amount <= 0)
                    {
                        Pos[user.UserId] = userBal.Amount;
                    }
                    else
                    {
                        Neg[user.UserId] = userBal.Amount;
                    }
                }
            }
            foreach (var Payer in Pos)
            {
                foreach (var Receiver in Neg)
                {
                    decimal amount = Math.Min(Math.Abs(Payer.Value), Math.Abs(Receiver.Value));
                    transactions.Add(new Transaction { FromUserId = Payer.Key, ToUserId = Receiver.Key, Amount = amount });

                    // Update balance
                    Pos[Payer.Key] -= amount;
                    Neg[Receiver.Key] = amount;

                    //Remove or update entries if fullly settle

                    //if (Pos[Payer.Key] == 0)
                    //{
                    //    Pos.Remove(Payer.Key);
                    //}
                    //if (Neg[Receiver.Key] == 0) 
                    //{ 
                    //    Neg.Remove(Receiver.Key);
                    //}
                }
            }
            res.Data = transactions;
            return res;
            //return transactions1;


    }
    

    public async Task<Response> DeleteBalance(int balanceId)
        {
            Response res = new Response();
            res.Status = true;

            if(balanceId<=0)
            {
                res.Message = "Invalid Balance ID";
                return res;
            }
            var balance = await _dbContext.Balances.FirstOrDefaultAsync(b => b.Id == balanceId);
            if (balance == null)
            {
                res.Message = "Balance doesn't exist.Please provide valid balance id to delete.";
                return res;
            }
            _dbContext.Balances.Remove(balance);
            await _dbContext.SaveChangesAsync();
            res.Message = "Balance Id " + balanceId + " Deleted Successfully.";
            return res;

        }


    }
}
