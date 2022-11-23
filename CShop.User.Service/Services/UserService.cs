using AutoMapper;
using CShop.User.Database.Model;
using CShop.User.Repository.Contracts;
using CShop.User.Service.Contracts;
using CShop.User.Service.CustomException;
using CShop.User.Service.DTO;
using CShop.User.Service.Handler;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordH;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordH, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordH = passwordH;
            _httpClient = httpClient;
        }
        public async Task<List<ShowUserDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(user => _mapper.Map<ShowUserDTO>(user)).ToList();
        }
        public async Task<UserDTO?> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);

            if (user == null) throw new NotFoundHandler("No user was found with that Username!");
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<ShowUserDTO?> GetUserByUserId(int Id)
        {
            var user = await _userRepository.GetUserByUserId(Id);

            if (user == null) throw new NotFoundHandler("No user was found with that Username!");
            return _mapper.Map<ShowUserDTO>(user);
        }
        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null) throw new NotFoundHandler("No user was found with that email!");
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO?> PostUser(UserDTO request)
        {
            //_userdtovalidator.ValidateDTO(request);

            var user = _mapper.Map<UserModel>(request);
            user.LastModifiedTime = DateTime.UtcNow;
            user.CreationTime = DateTime.UtcNow;
            var newUser = await _userRepository.PostUser(user);
            var userDto = _mapper.Map<UserDTO>(newUser);
            return userDto;
        }

        public async Task<ShowUserDTO?> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            var fetchedUser = await _userRepository.GetUserByUserId(id);
            if (fetchedUser == null) throw new NotFoundHandler("No user was found with that ID!");
            //_userdtovalidator.ValidateDTO(updateUserDto);
            var currUser = _passwordH.GetLoggedInUsername();
            if (currUser == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");
            if (currUser != fetchedUser.UserName) throw new ForbiddenHandler("You don't have the permission!");



            var creationTime = _passwordH.GetTokenCreationTime();
            if (creationTime == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");

            DateTime tokenCreationTime = Convert.ToDateTime(creationTime);

            if (tokenCreationTime.ToOADate() + 86400000 < fetchedUser.LastModifiedTime.ToOADate())
                throw new UnauthorisedHandler("JWT Expired! Login again!");

            UserModel mappedUser = _mapper.Map<UserModel>(updateUserDto);


            if (updateUserDto.Password != null && updateUserDto.Password != "")
            {
                if (updateUserDto.Password.Length < 8) throw new BadRequestHandler("Password length must be at least 8");
                Tuple<byte[], byte[]> passwordObject = _passwordH.HashPassword(updateUserDto.Password);
                byte[] passwordSalt = passwordObject.Item2;
                byte[] passwordHash = passwordObject.Item1;
                mappedUser.PasswordHash = passwordHash;
            }
            mappedUser.LastModifiedTime = DateTime.UtcNow;

            var newUser = await _userRepository.UpdateUser(id, mappedUser);
            var userDto = _mapper.Map<ShowUserDTO>(newUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUser(string accessToken, int id)
        {
            var fetchedUser = await _userRepository.GetUserByUserId(id);
            if (fetchedUser == null) throw new NotFoundHandler("No user was found with that ID!");
            var currUser = _passwordH.GetLoggedInUsername();
            if (currUser == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");
            if (currUser != fetchedUser.UserName) throw new ForbiddenHandler("You don't have the permission!");



            var creationTime = _passwordH.GetTokenCreationTime();
            if (creationTime == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");

            DateTime tokenCreationTime = Convert.ToDateTime(creationTime);

            if (DateTime.Compare(tokenCreationTime, fetchedUser.LastModifiedTime) < 0)
                throw new UnauthorisedHandler("Session Expired! Login again!");

            // InterService Communication Starts
            //_httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

            //using HttpResponseMessage response = await _httpClient.DeleteAsync($"https://cshopapigateway.azurewebsites.net/api/carts/{id}");
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            // InterService Communication Ends
            return await _userRepository.DeleteUser(id);
        }
    }
}
