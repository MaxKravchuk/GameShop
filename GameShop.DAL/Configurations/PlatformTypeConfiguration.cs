﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class PlatformTypeConfiguration : EntityTypeConfiguration<PlatformType>
    {
        public PlatformTypeConfiguration()
        {
            ToTable("PlatformTypes");

            HasKey(x => x.Id);

            Property(x => x.Type)
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("IX_Type") { IsUnique = true }));

            Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
