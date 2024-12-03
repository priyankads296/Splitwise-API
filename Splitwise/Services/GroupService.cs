using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Models;
using Response = Splitwise.Models.Response;

namespace Splitwise.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _dbContext;
       
        public GroupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
           
        }

        public async Task<Response> CreateGroup(Group group)
        {
            Response response = new Response();
            var existingGroup = await _dbContext.Groups
             .Include(u => u.Users) // Include the Users related to the Group
             .Include(g => g.GroupDetails) // Include the GroupDetails related to the Group
             .FirstOrDefaultAsync(u => u.GroupDetails.Name == group.GroupDetails.Name);
            var usersToAdd = new List<User>();
            var existingUsersInDb=await _dbContext.Users.ToListAsync();
            
            if(existingGroup != null)
            {
                response.Status = false;
                response.Message = "Group already exists.";
                return response;
            }
            foreach(var user in group.Users)
            {
                var existingUser= existingUsersInDb.FirstOrDefault(u=>u.Name.ToLower()==user.Name.ToLower() && u.Email.ToLower()==user.Email.ToLower());
                if(existingUser!=null)
                {
                    usersToAdd.Add(existingUser);
                }
                else
                {
                    usersToAdd.Add(user);
                }
            }
            group.Users = usersToAdd;
            await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();
            response.Status = true;
            response.Message = "Group created Successfully";
            response.Data = group;
            return response;

        }
        public async Task<Response> GetAllGroups()
        {
            Response res = new Response();
            var groups = await _dbContext.Groups.Include(u => u.GroupDetails).Include(u => u.Users).ToListAsync();
            res.Status = true;

            // var groups = await _dbContext.Groups.ToListAsync();
            if (groups == null) {
                res.Message = "Group not found";
                return res;
            }
            res.Data = groups;
            res.Message = "Group fetched Successfully....";
            return res;
        }
        public async Task<Response> GetGroupById(int id)
        {
            Response res = new Response();
            var existingGroup = await _dbContext.Groups.Include(u => u.Users)
                               .Include(g => g.GroupDetails)
                               .FirstOrDefaultAsync(u => u.GroupId==id);
            if (existingGroup == null)
            {
                res.Status = false;
                res.Message = "Group not found";
                return res;
            }
            res.Status = true;
            res.Data = existingGroup;
            res.Message = "Group fetched by id Successfully";
            return res;

        }
        public async Task<Response> AddUserToGroup(string groupName, User user)
        {
            Response res = new Response();
            var existingGroup = await _dbContext.Groups.Include(u => u.Users)
                               .Include(g => g.GroupDetails)
                               .FirstOrDefaultAsync(u => u.GroupDetails.Name.ToLower() == groupName.ToLower());

            if (existingGroup == null) {
                res.Status = false;
                res.Message = "Group not found";
                return res;

            }
            if (existingGroup.Users.Any(u => u.Name.ToLower() == user.Name.ToLower()))
            {
                res.Message = "User already exists in the group.";
                res.Status = false;
                return res;
            }

            //if (existingGroup != null && existingGroup.Users != null)
            //{
            //    var usersInGroup = group.Users;

            //    bool isUserMatch = existingGroup.Users
            //                    .Any(existingUser => usersInGroup
            //                    .Any(newUser => newUser.Name.ToLower() == existingUser.Name.ToLower())); //if user already exists in the group

            //    if (isUserMatch)
            //    {
            //        response.Message = "A user in the new group matches an existing user.";
            //        response.Status = false;
            //        return response;
            //    }
            //}


            existingGroup.Users.Add(user);
            res.Status = true;
            res.Data = existingGroup;
            await _dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<Response> DeleteGroup(int id)
        {
            Response res = new Response();
            var group = await _dbContext.Groups.FirstOrDefaultAsync(u => u.GroupId == id);
            if (group == null) { 
                res.Message="Group not found. Please enter valid Group.";
                res.Status = true;
                return res;
            }

            //check if there are dependent entitites
            var dependentEntities = await _dbContext.Expenses.Where(e => e.GroupName.ToLower() == group.GroupDetails.Name.ToLower()).ToListAsync();
            //var dependentUsers = await _dbContext.Users.Where(e => e.GroupId == id).ToListAsync();

            foreach (var entity in dependentEntities)
            {
                entity.GroupName = null;
                res.Status = true;
                res.Message = "Group has dependent entities. Please delete the dependent entities first.";
                return res;
            }
            //foreach (var entity in dependentUsers)
            //{
            //    entity.GroupId = null;
            //    return BadRequest("Group has dependent entities. Please delete the dependent entities first.");
            //}
            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
            res.Message = "Deleted Successfully!";
            return res;
        }

    }
}
