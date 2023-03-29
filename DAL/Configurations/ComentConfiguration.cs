using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class ComentConfiguration : EntityTypeConfiguration<Coment>
    {
        public ComentConfiguration()
        {
            this.ToTable("Coments");

        }
    }
}
