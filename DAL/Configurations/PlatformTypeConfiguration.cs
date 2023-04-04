using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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

            this.HasKey(x => x.Id);

            this
                .Property(x=>x.Type)
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Type") { IsUnique = true }));

            this
                .Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
