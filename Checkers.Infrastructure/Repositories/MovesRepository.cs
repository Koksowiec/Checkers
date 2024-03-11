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
    public class MovesRepository : IMovesRepository
    {
        private readonly CheckersDbContext _dbContext;

        public MovesRepository(CheckersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(string gameId)
        {
            var move = new Moves()
            {
                GameId = Int32.Parse(gameId),
                P1_Moves = "",
                P2_Moves = ""
            };

            await _dbContext.AddAsync(move);
            await Commit();
        }

        public async Task<Moves>? GetMovesById(string gameId)
        {
            return await _dbContext.Moves.FirstOrDefaultAsync(g => g.GameId == Int32.Parse(gameId));
        }

        public async Task UpdateP1Moves(string gameId, string moves)
        {
            var movesFromDb = await GetMovesById(gameId);

            if(movesFromDb != null)
            {
                movesFromDb.P1_Moves += moves + ";";

                _dbContext.Update(movesFromDb);
                await Commit();
            }
        }

        public async Task UpdateP2Moves(string gameId, string moves)
        {
            var movesFromDb = await GetMovesById(gameId);

            if (movesFromDb != null)
            {
                movesFromDb.P2_Moves += moves + ";";

                _dbContext.Update(movesFromDb);
                await Commit();
            }
        }
    }
}
