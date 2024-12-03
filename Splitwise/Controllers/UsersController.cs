using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Models;
using Splitwise.Services;
using System.Text.RegularExpressions;

namespace Splitwise.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public UsersController(ApplicationDbContext dbContext,IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;

        }

        //GET API

        [HttpGet]
        
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if (response.Status == false)
                return NotFound(response.Message);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
                return BadRequest("Enter valid id");
            var response = await _userService.GetUserById(id);
            if (response.Status == false)
                return NotFound(response.Message);
            return Ok(response);

        }
        [HttpGet("GetUserByIds")]
        public async Task<IActionResult> GetUserByIds([FromQuery]int[] userIds)
        {
            if (userIds == null)
                return BadRequest("Enter valid id");
            var response = await _userService.GetUserByIds(userIds);
            if (response.Status == false)
                return NotFound(response.Message);
            return Ok(response);
        }
        [HttpGet("groupId")]
        public async Task<IActionResult> GetUserByGroup(int groupId)
        {
            if (groupId <= 0)
                return BadRequest("Enter valid id");
            var response = await _userService.GetUserByGroup(groupId);
            if (response.Status == false)
                return NotFound(response.Message);
            return Ok(response);
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            var response =await _userService.CreateUser(user);
            if(response.Status==false)
                return BadRequest(response.Message);
            return Ok(response);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.Status == false)
                return BadRequest(response.Message);
            return Ok(response);
        }
    }
}
