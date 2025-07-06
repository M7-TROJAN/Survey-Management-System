using System.Reflection;

namespace SurveyBasket.Web.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // this will apply all configurations in the current assembly

        base.OnModelCreating(modelBuilder);
        // Add any additional model configurations here
    }
}