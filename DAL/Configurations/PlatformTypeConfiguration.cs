using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class PlatformTypeConfiguration : EntityTypeConfiguration<PlatformType>
    {
        public PlatformTypeConfiguration()
        {
            this.ToTable("PlatformTypes");

            this.HasKey(x => x.Type);

            this
                .Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
