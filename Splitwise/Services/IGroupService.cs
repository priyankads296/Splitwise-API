using Splitwise.Models;

namespace Splitwise.Services
{
    public interface IGroupService
    {
        public Task<Response> CreateGroup(Group group);
        public Task<Response> GetGroupById(int groupId);
        public Task<Response> GetAllGroups();
        public Task<Response> AddUserToGroup(string groupName, User user);
        public Task<Response> DeleteGroup(int id);
    }
}
