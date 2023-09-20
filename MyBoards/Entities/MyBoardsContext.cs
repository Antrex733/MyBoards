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
        public DbSet<WorkItemState> WorkItemStates { get; set; }
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
                eb.Property(p => p.Priority).HasDefaultValue(1);
                eb.Property(p => p.State).IsRequired().HasMaxLength(50);

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

                eb.HasOne(a => a.State)
                .WithMany(a => a.WorkItems)
                .HasForeignKey(a => a.StateId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(p => p.CreateDate).HasDefaultValueSql("getutcdate()");
                eb.Property(p => p.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<WorkItemState>(
                w => w.Property(x => x.)
                


        }

    }
}
