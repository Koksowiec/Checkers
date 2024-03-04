using Checkers.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Checkers.Infrastructure.Persistance
{
    public class CheckersDbContext : DbContext
    {
        public CheckersDbContext(DbContextOptions<CheckersDbContext> options) : base(options)
        {

        }

        DbSet<Game> Games { get; set; }
        DbSet<GameDetails> GamesDetails { get; set; }
        DbSet<Moves> Moves { get; set; }
    }
}
