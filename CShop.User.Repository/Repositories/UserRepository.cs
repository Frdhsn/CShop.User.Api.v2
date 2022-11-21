using CShop.User.Database.Context;
using CShop.User.Database.Model;
using CShop.User.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Repository.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<UserModel?> GetUserByUsername(string username)
        {
            return await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync<UserModel>();
        }
        public async Task<UserModel?> GetUserByUserId(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }
        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync<UserModel>();
        }
        public async Task<UserModel?> PostUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel?> UpdateUser(int id, UserModel user)
        {
            var updatedUser = await _context.Users.FindAsync(id);

            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.UserName = user.UserName;
            updatedUser.Name = user.Name;
            updatedUser.Email = user.Email;
            updatedUser.Address = user.Address;
            updatedUser.LastModifiedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return updatedUser;
        }
        public async Task<Boolean> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<UserModel?> AddItem(int id, Product product)
        {
            var updatedUser = await _context.Users.FindAsync(id);

            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Cart.Add(product);
            updatedUser.LastModifiedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return updatedUser;
        }
        public async Task<UserModel> CreateCart(int id, ICollection<Product> cart)
        {
            var updatedUser = await _context.Users.FindAsync(id);

            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Cart = cart;
            updatedUser.LastModifiedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return updatedUser;
        }
        public async Task<ICollection<Product>> GetCart(int id)
        {
            var updatedUser = await _context.Users.FindAsync(id);

            if (updatedUser == null)
            {
                return null;
            }
            return updatedUser.Cart;
        }
        public async Task<Boolean> ResetCart(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }
            user.Cart.Clear();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
