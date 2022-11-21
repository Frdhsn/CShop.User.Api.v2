using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Database.Model
{
    public class Product
    {
        public int Id { get; set; }
        public int Count { get; set; }
        
        // need this?
        //public virtual UserModel User { get; set; }
    }
}
