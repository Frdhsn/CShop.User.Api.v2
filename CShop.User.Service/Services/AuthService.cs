using AutoMapper;
using CShop.User.Database.Model;
using CShop.User.Repository.Contracts;
using CShop.User.Service.Contracts;
using CShop.User.Service.CustomException;
using CShop.User.Service.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CShop.User.Service.Services
{
    

    public class AuthService: IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordH;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;
        public AuthService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordH, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordH = passwordH;
            _httpClient = httpClient;
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

            //string token = "Bearer " + userDTO.Token;
            string token = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJUcnVlIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiMTEvMjUvMjAyMiAxMDo0ODowNyIsImV4cCI6MTY3MzY5MzI4N30.HN8-z7rJpDsinuK90qbeq3hkneXPZbwQhqXixIMSUF0";
            // Interservice comm: create Cart
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var url = "https://cshopapigateway.azurewebsites.net/api/carts";
            //var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive= true };

            //var payload = { "userId": newUser.Id };
            dynamic payload = new JObject();
            payload.userid = newUser.Id;
            var newUserSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var stringContent = new StringContent(newUserSerialized, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

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
