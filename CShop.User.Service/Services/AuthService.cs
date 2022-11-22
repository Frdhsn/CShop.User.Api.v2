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
    public class AuthService: IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordH;
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordH)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordH = passwordH;
        }

        public async Task<UserDTO> SignUp(SignUpDTO req)
        {
            //_signupdtovalidator.ValidateDTO(req);

            Tuple<byte[], byte[]> hashedPassword = _passwordH.HashPassword(req.Password);

            var user = _mapper.Map<UserModel>(req);
            user.PasswordHash = hashedPassword.Item2;
            user.PasswordSalt = hashedPassword.Item1;
            user.LastModifiedTime = DateTime.UtcNow;
            user.CreationTime = DateTime.UtcNow;

            var newUser = await _userRepository.PostUser(user);
            var userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.Token = _passwordH.CreateToken(newUser);
            return userDTO;
        }
        public async Task<UserDTO> Login(LoginDTO req)
        {
            //_logindtovalidator.ValidateDTO(req);

            var newUser = await _userRepository.GetUserByEmail(req.Email);
            if (newUser == null) throw new UnauthorisedHandler("Incorrect email or password!");
            bool ret = _passwordH.VerifyHash(req.Password, newUser.PasswordHash, newUser.PasswordSalt);
            if (!ret) throw new UnauthorisedHandler("Incorrect email or password!");
            var token = _passwordH.CreateToken(newUser);

            var userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.Token = token;
            return userDTO;
        }

    }
}
