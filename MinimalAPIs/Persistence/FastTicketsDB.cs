using Microsoft.EntityFrameworkCore;
using Models;

namespace Persistence;

public class FastTicketsDB : DbContext
{
    public FastTicketsDB(DbContextOptions<FastTicketsDB> options)
        : base(options) { }

    public DbSet<Show> Shows => Set<Show>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
}
