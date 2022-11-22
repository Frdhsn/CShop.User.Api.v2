using CShop.User.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Contracts
{
    public interface IUserService
    {

        Task<List<UserDTO>> GetAllUsers();
        Task<UserDTO?> GetUserByUserId(int Id);
        Task<UserDTO?> GetUserByUsername(string username);
        Task<UserDTO?> GetUserByEmail(string email);
        Task<UserDTO?> PostUser(UserDTO user);
        Task<UserDTO?> UpdateUser(int Id, UpdateUserDTO user);
        Task<Boolean> DeleteUser(int Id);
    }
}
