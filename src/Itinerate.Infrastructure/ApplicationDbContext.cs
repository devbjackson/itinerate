using Microsoft.EntityFrameworkCore;
using Itinerate.Domain;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<Event> Events { get; set; }
}