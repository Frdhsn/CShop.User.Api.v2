using CShop.User.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.DTO
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Product> Cart { get; set; }
    }
}
