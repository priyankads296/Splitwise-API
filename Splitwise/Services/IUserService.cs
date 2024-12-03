using Microsoft.AspNetCore.Mvc;
using Splitwise.Models;

namespace Splitwise.Services
{
    public interface IUserService
    {
        public Task<Response> GetAllUsers();
        public Task<Response> GetUserById(int id);
        public Task<Response> GetUserByIds(int[] userIds);
        public Task<Response> GetUserByGroup(int groupId);
        public Task<Response> CreateUser(User user);

        public Task<Response> DeleteUser(int id);
    }
}
