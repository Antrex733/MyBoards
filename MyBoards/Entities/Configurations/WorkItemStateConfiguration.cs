using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemStateConfiguration : IEntityTypeConfiguration<WorkItemState>
    {
        public void Configure(EntityTypeBuilder<WorkItemState> w)
        {
            w.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(50);

            w.HasData(
                new WorkItemState { Id = 1, Value = "To Do" },
                new WorkItemState { Id = 2, Value = "Doing" },
                new WorkItemState { Id = 3, Value = "Done" }
                );
        }
    }
}
