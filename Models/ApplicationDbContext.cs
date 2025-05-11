using Microsoft.EntityFrameworkCore;

namespace DiplomaProject_ITMO.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackModel>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<User>()
           .HasIndex(u => u.Username)
           .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


        }



    }


}