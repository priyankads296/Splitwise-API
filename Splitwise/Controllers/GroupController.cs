using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Models;
using Splitwise.Services;

namespace Splitwise.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private readonly IGroupService _groupService;

        public GroupController(ApplicationDbContext dbContext,IGroupService groupService)
        {
            _dbContext = dbContext;
            _groupService = groupService;

        }
        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            var newGroup =await _groupService.CreateGroup(group);
            await _dbContext.SaveChangesAsync();
            return Ok(newGroup);


        }
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroups();
           // var groups = await _dbContext.Groups.ToListAsync();
            if (groups == null) { return NotFound(); }

            return Ok(groups);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetGroupById(id);
            if (group == null) { return NotFound(); };
            return Ok(group);
        }

        [HttpPost("{groupName}")]
        public async Task<IActionResult> AddUserToGroup(string groupName, User user)
        {
            var response=await _groupService.AddUserToGroup(groupName, user);
            return Ok(response);    
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group=await _groupService.DeleteGroup(id);
            if (id == 0)
                return BadRequest("Enter valid id.");
            await _dbContext.SaveChangesAsync();
            return Ok(group);
        }

    }
}
