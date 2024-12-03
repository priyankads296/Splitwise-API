using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Models;
using System.Text.RegularExpressions;

namespace Splitwise.Services
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<Response> CreateUser(User user)
        {
            Response response = new Response();
            var existingUser= await _dbContext.Users
                                .FirstOrDefaultAsync(u=>u.Email.ToLower()==user.Email.ToLower());
            if (existingUser != null)
            {
                response.Status = false;
                response.Message = "User already exists...";
                return response;
            }
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            response.Status = true;
            response.Message = "User added Successfully...";
            response.Data = _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());
            return response;

        }

        public async Task<Response> DeleteUser(int id)
        {
           
            Response response = new Response();
            response.Status = true; ;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) 
            {
                response.Status = false;
                response.Message = "User doesn't exists...";
                return response;
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            response.Message = "User deleted Successfully...";

            return response;
        }

        public async Task<Response> GetAllUsers()
        {
           Response response=new Response();
            response.Status = true;
           var users = await _dbContext.Users.ToListAsync();
           if(users==null)
            {
                response.Status = false;
                response.Message = "No users found..";
            }
            response.Message = "Users fetched Successfully!";
            response.Data = users;
            return response;
        }

        public async Task<Response> GetUserByGroup(int groupId)
        {
            Response response = new Response();
            response.Status = true; 
            var group = await _dbContext.Groups.Include(u => u.Users).FirstOrDefaultAsync(u => u.GroupId == groupId);
            if (group == null)
            {
                response.Status = false;
                response.Message = "No group found..";
                return response;
            }
            if(group.Users==null)
            {
                response.Status = false;
                response.Message = "No user found in the group..";
                return response;
            }
            var users = group.Users;
            response.Data = users;
            response.Message = "Users fetched Successfully..";
            return response;
        }

        public async Task<Response> GetUserById(int id)
        {
            Response response = new Response();
            response.Status = true;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                response.Status = false;
                response.Message = "No user found..";
                return response;
            }
            response.Status = true;
            response.Message = "User fetched Successfully..";
            response.Data = user;
            return response;
        }

        public async Task<Response> GetUserByIds(int[] userIds)
        {
            Response response = new Response();
            response.Status = true;
            var users= await _dbContext.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            if (users == null)
            {
                response.Status = false;
                response.Message = "No user found..";
                return response;
            }
            response.Status = true;
            response.Message = "User fetched Successfully..";
            response.Data = users;
            return response;
        }
    }
}
