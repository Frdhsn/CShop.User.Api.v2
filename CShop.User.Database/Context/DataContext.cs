using CShop.User.Database.Configuration;
using CShop.User.Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Database.Context
{
    public class DataContext: DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Product> Cart { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new UserConfig().Configure(builder.Entity<UserModel>());
            new CartConfig().Configure(builder.Entity<Product>());
        }
    }
}
