using Microsoft.EntityFrameworkCore;

namespace DiplomaProject_ITMO.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<AdditionalCalculationsModel> AdditionalCalculations { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; } 
    }
}
