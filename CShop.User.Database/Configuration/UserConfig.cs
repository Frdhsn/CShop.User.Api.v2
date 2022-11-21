using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.User.Database.Model;

namespace CShop.User.Database.Configuration
{
    public class UserConfig: IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).IsRequired();
            builder.HasIndex(user => user.Id).IsUnique();

            builder.Property(user => user.UserName).IsRequired();
            builder.HasIndex(user => user.UserName).IsUnique();

            builder.Property(user => user.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(user => user.Email).IsUnique();

            builder.Property(user => user.Address).IsRequired().HasMaxLength(256);
            builder.Property(user => user.isAdmin).IsRequired();

            builder.Property(user => user.PasswordHash).IsRequired();
            builder.Property(user => user.PasswordSalt).IsRequired();

            builder.Property(user => user.CreationTime).IsRequired();

            builder.Property(user => user.LastModifiedTime).IsRequired();
        }
    }
}
