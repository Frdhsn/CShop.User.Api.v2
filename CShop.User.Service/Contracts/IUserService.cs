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

        Task<List<ShowUserDTO>> GetAllUsers();
        Task<ShowUserDTO?> GetUserByUserId(int Id);
        Task<UserDTO?> GetUserByUsername(string username);
        Task<UserDTO?> GetUserByEmail(string email);
        Task<UserDTO?> PostUser(UserDTO user);
        Task<ShowUserDTO?> UpdateUser(int Id, UpdateUserDTO user);
        Task<Boolean> DeleteUser(int Id);
    }
}
