using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            HasKey(x => x.Id);

            Property(x => x.NickName)
                .IsRequired();

            Property(x => x.PasswordHash)
                .IsRequired();

            HasOptional(x => x.UserRole)
                .WithMany(x => x.UsersRole)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.RefreshToken)
                .WithRequired(x => x.User);

            HasMany(x => x.Orders)
                .WithRequired(x => x.Customer)
                .HasForeignKey(x => x.CustomerId)
                .WillCascadeOnDelete(false);
        }
    }
}
