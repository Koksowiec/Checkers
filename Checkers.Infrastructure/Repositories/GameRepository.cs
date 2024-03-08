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

        public async Task CreateAsync(string gameId, string p1Name = "")
        {
            var game = new Game()
            {
                Id = Int32.Parse(gameId),
                P1 = p1Name,
                P2 = "",
                Winner = "",
                StartingPlayer = ""
            };

            await _dbContext.AddAsync(game);
            await Commit();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _dbContext.Games.ToListAsync();
        }

        public async Task<Game>? GetGameById(string gameId)
        {
            return await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == Int32.Parse(gameId));
        }

        public async Task UpdateGameP1(string gameId, string name)
        {
            var game = await GetGameById(gameId);
            if(game != null)
            {
                game.P1 = name;

                _dbContext.Update(game);
                await Commit();
            }
        }

        public async Task UpdateGameP2(string gameId, string name)
        {
            var game = await GetGameById(gameId);
            if (game != null)
            {
                game.P2 = name;

                _dbContext.Update(game);
                await Commit();
            }
        }
    }
}
