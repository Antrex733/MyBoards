using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> eb)
        {

            eb.HasOne(a => a.State)
                .WithMany(a => a.WorkItems)
                .HasForeignKey(a => a.StateId);

            eb.Property(p => p.Area).HasColumnType("varchar(200)");
            eb.Property(p => p.IterationPath).HasColumnName("Iteration_Path");
            eb.Property(p => p.Priority).HasDefaultValue(1);
            //eb.Property(p => p.State).IsRequired().HasMaxLength(50);

            eb.HasMany(a => a.Comments)
            .WithOne(p => p.WorkItem)
            .HasForeignKey(u => u.WorkItemId);

            eb.HasOne(a => a.User)
            .WithMany(u => u.WorkItems)
            .HasForeignKey(x => x.UserId);

            eb.HasMany(a => a.Tags)
            .WithMany(b => b.WorkItems)
            .UsingEntity<WorkItemTag>(
                w => w.HasOne(wit => wit.Tag)
                .WithMany()
                .HasForeignKey(wit => wit.TagId),

                w => w.HasOne(wit => wit.WorkItem)
                .WithMany()
                .HasForeignKey(wit => wit.WorkItemId),

                wit =>
                {
                    wit.HasKey(x => new { x.TagId, x.WorkItemId });
                    wit.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                });
        }
    }
}
