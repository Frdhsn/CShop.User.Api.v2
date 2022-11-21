using CShop.User.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Database.Configuration
{
    public class CartConfig: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(product => product.Id);
            builder.Property(product => product.Id).IsRequired();
            builder.HasIndex(product => product.Id).IsUnique();

            builder.Property(product => product.Count).IsRequired();

        }
    }
}
