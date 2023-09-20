using Microsoft.EntityFrameworkCore;

namespace MyBoards.Entities
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(p => p.State).IsRequired();
                eb.Property(p => p.Area).HasColumnType("varchar(200)");
                eb.Property(p => p.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(p => p.Effort).HasColumnType("decimal(5,2)");
                eb.Property(p => p.EndDate).HasPrecision(3);
                eb.Property(p => p.Activity).HasMaxLength(200);
                eb.Property(p => p.RemaningWork).HasPrecision(14, 2);
            });

        }

    }
}
