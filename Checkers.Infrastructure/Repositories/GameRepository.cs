using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Checkers.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly CheckersDbContext _dbContext;

        public GameRepository(CheckersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Game game)
        {
            await _dbContext.AddAsync(game);
            await Commit();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _dbContext.Games.ToListAsync();
        }
    }
}
