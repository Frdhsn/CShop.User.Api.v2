using CShop.User.Database.Model;
using CShop.User.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Contracts
{
    public interface ICartService
    {
        //Task<List<CartDTO>> GetCarts();
        Task<ICollection<Product>> GetCartByUserId(int id);
        //Task<CartDTO> GetCartByUserName(string Username);
        Task<CartDTO> CreateCart(CartDTO body);
        Task<CartDTO> AddItem(int id, Product body);
        Task<bool?> ResetCart(int id);
    }
}
