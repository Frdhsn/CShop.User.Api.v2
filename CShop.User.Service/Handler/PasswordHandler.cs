using CShop.User.Database.Model;
using CShop.User.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Handler
{
    public class PasswordHandler: IPasswordHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasswordHandler(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetLoggedInId()
        {
            int Id = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                Id = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return Id;
            }
            return Id;
        }
        public string GetLoggedInUsername()
        {
            var username = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return username;
        }

        public string GetTokenCreationTime()
        {
            var creationTime = String.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                creationTime = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Expiration).Value;
                return creationTime;
            }
            return creationTime;
        }
        public Tuple<byte[], byte[]> HashPassword(string password)
        {
            byte[] passwordSalt, passwordHash;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return new Tuple<byte[], byte[]>(passwordSalt, passwordHash);
        }

        public bool VerifyHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("EnvironmentVariable:token").Value));

            var createdCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(50),
                signingCredentials: createdCredential);

            var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            //var ret = JSON
            //return JsonConvert.SerializeObject(finalToken).ToString();
            return finalToken;
        }
        public Boolean HttpContextExists()
        {
            return _httpContextAccessor.HttpContext != null;
        }
        public void DeleteToken()
        {
            _httpContextAccessor.HttpContext = null;
        }

    }
}
