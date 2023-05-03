using System.Data.Entity.ModelConfiguration;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            ToTable("Comments");

            HasKey(x => x.Id);

            Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            Property(x => x.Body)
                .HasMaxLength(255)
                .IsRequired();

            HasRequired(x => x.Game)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.GameId)
                .WillCascadeOnDelete(false);
        }
    }
}
