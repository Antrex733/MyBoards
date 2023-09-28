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
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<WorkItemTag> WorkItemTag { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
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
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(p => p.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(p => p.UpdatedDate).ValueGeneratedOnUpdate();
                eb.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<WorkItemState>(w =>
            {
            w.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(50);

            w.HasData(
                new WorkItemState { Id = 1, Value = "To Do" },
                new WorkItemState { Id = 2, Value = "Doing" },
                new WorkItemState { Id = 3, Value = "Done" }
                );
            });
            modelBuilder.Entity<Epic>(e =>
                e.Property(x => x.EndDate)
                .HasPrecision(3));

            modelBuilder.Entity<Issue>(i =>
                i.Property(x => x.Efford)
                .HasColumnType("decimal(5,2)"));

            modelBuilder.Entity<Task>(t =>
            {
                t.Property(x => x.Activity)
                    .HasMaxLength(200);
                t.Property(x => x.RemaningWork)
                    .HasPrecision(14, 2);
            });
            modelBuilder.Entity<Tag>(a =>
            {
                a.HasData(
                    new Tag() { Id = 1, Value = "Web" },
                    new Tag() { Id = 2, Value = "Ui" },
                    new Tag() { Id = 3, Value = "Desktop" },
                    new Tag() { Id = 4, Value = "API" },
                    new Tag() { Id = 5, Value = "Service" }
                    );
            });
        }

    }
}
