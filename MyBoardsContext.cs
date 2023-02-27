using Microsoft.EntityFrameworkCore;
using System.Runtime;

namespace EF_Study.Encje
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Adress> Addresses { get; set; }
        public DbSet<State> States{ get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>(eb => eb.Property(x => x.Efford).HasColumnType("decimal(5,2"));

            modelBuilder.Entity<Epic>(eb => eb.Property(x => x.EndDate).HasPrecision(3));

            modelBuilder.Entity<Task>(eb =>
            {
                eb.Property(x => x.RemWork).HasPrecision(14, 2);
                eb.Property(x => x.Activity).HasMaxLength(200);
            });

            modelBuilder.Entity<WorkItem>(eb => 
            {
                eb.Property(x => x.State).IsRequired();
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Priority).HasDefaultValue(1);
                eb.HasMany(x => x.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(x => x.WorkItemId);

                eb.HasOne(x => x.Author)
                .WithMany(x => x.WorkItems)
                .HasForeignKey(x => x.AuthorId);

                eb.HasOne(x => x.State)
                .WithMany(x => x.WorkItems)
                .HasForeignKey(x => x.StateId);

                eb.HasMany(x => x.Tags)
                .WithMany(c => c.WorkItems)
                .UsingEntity<WorkItemTag>(
                    x => x.HasOne(w => w.Tag)
                    .WithMany()
                    .HasForeignKey(w => w.TagId),

                    x => x.HasOne(w => w.WorkItem)
                    .WithMany()
                    .HasForeignKey(w => w.WorkItemId),

                    x =>
                    {
                        x.HasKey(x => new { x.TagId, x.WorkItemId });
                        x.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                    }

                    );
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>()
                .HasOne(eb => eb.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Adress>(a => a.UserId);

            modelBuilder.Entity<State>(eb =>
            {
                eb.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
            });
            
        }

    }
}
