﻿using CShop.User.Service.Contracts;
using CShop.User.Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CShop.User.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByUserId(int id)
        {
            var user = await _userService.GetUserByUserId(id);
            if (user == null)
                return BadRequest("User not found!");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserDTO request)
        {
            var userDto = await _userService.PostUser(request);
            if (userDto == null)
                return BadRequest("Something went wrong! Can't create a new user!");

            return CreatedAtAction(nameof(PostUser), userDto.Id, userDto);
        }
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO updatedUserDto)
        {
            if (id != updatedUserDto.Id)
                return BadRequest("Something went wrong! Can't update the user.");
            var updatedDTO = await _userService.UpdateUser(id, updatedUserDto);

            if (updatedDTO == null)
                return BadRequest("Something went wrong! Can't update the user.");
            return Ok(updatedDTO);
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            //create a token using this user's info
            // send request to order service
            //now delete user
            var deletedUser = await _userService.DeleteUser(accessToken.ToString(), id);
            return NoContent();
        }
    }
}
