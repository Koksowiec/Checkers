using Checkers.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Checkers.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        DbSet<Game> Games { get; set; }
        DbSet<GameDetails> GamesDetails { get; set; }
        DbSet<Moves> Moves { get; set; }
    }
}
