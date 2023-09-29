using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards.Entities.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> t)
        {
            t.Property(x => x.Activity)
                    .HasMaxLength(200);
            t.Property(x => x.RemaningWork)
                .HasPrecision(14, 2);
        }
    }
}
