using CShop.User.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel?> GetUserByUserId(int id);
        Task<UserModel?> GetUserByUsername(string username);
        Task<UserModel?> GetUserByEmail(string email);
        Task<UserModel?> PostUser(UserModel user);
        Task<UserModel?> UpdateUser(int id, UserModel user);
        Task<Boolean> DeleteUser(int id);
        Task<UserModel?> AddItem(int id, Product product);
        Task<UserModel> CreateCart(int id, ICollection<Product>cart);
        Task<ICollection<Product>> GetCart(int id);
        Task<Boolean> ResetCart(int id);
    }
}
