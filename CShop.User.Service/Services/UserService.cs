using AutoMapper;
using CShop.User.Database.Model;
using CShop.User.Repository.Contracts;
using CShop.User.Service.Contracts;
using CShop.User.Service.CustomException;
using CShop.User.Service.DTO;
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
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordH) {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordH = passwordH;
        }
        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(user => _mapper.Map<UserDTO>(user)).ToList();
        }
        public async Task<UserDTO?> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);

            if (user == null) throw new NotFoundHandler("No user was found with that Username!");
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO?> GetUserByUserId(int Id)
        {
            var user = await _userRepository.GetUserByUserId(Id);

            if (user == null) throw new NotFoundHandler("No user was found with that Username!");
            return _mapper.Map<UserDTO>(user);
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

        public async Task<UserDTO?> UpdateUser(int id, UserDTO updateUserDto)
        {
            //_userdtovalidator.ValidateDTO(updateUserDto);
            // test for git
            var currUserId = _passwordH.GetLoggedInId();
            if (currUserId == -1) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");
            if (currUserId != id) throw new ForbiddenHandler("You don't have the permission!");

            var fetchedUser = await _userRepository.GetUserByUserId(id);

            if (fetchedUser == null) throw new NotFoundHandler("No user was found with that ID!");

            //var creationTime = _passwordH.GetTokenCreationTime();
            //if (creationTime == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");

            //DateTime tokenCreationTime = Convert.ToDateTime(creationTime);

            //if (DateTime.Compare(tokenCreationTime, fetchedUser.LastModifiedTime) < 0)
            //    throw new UnauthorisedHandler("JWT Expired! Login again!");

            UserModel mappedUser = _mapper.Map<UserModel>(updateUserDto);


            var updatedUser = await _userRepository.UpdateUser(id, mappedUser);
            // idk it's needed
            if (updatedUser == null) throw new NotFoundHandler("No user was found with that ID!");

            var userDto = _mapper.Map<UserDTO>(updatedUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUser(int id)
        {
            var fetchedUser = await _userRepository.GetUserByUserId(id);

            if (fetchedUser == null) throw new NotFoundHandler("No user was found with that ID!");
            var currUserId = _passwordH.GetLoggedInId();
            if (currUserId != id) throw new ForbiddenHandler("You don't have the permission!");

            //var creationTime = _passwordH.GetTokenCreationTime();
            //if (creationTime == null) throw new UnauthorisedHandler("You're not logged in! Please log in to get access.");

            //DateTime tokenCreationTime = Convert.ToDateTime(creationTime);

            //if (DateTime.Compare(tokenCreationTime, fetchedUser.LastModifiedTime) < 0)
            //    throw new UnauthorisedHandler("JWT Expired! Login again!");
            return await _userRepository.DeleteUser(id);
        }
    }
}
