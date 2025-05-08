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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackModel>()
                .HasKey(f => f.Id); // Указываем, что Id является первичным ключом

           
        }
    }
}
