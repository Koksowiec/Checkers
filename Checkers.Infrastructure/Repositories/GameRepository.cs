using Checkers.Domain.Interfaces;
using Checkers.Infrastructure.Persistance;
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
    }
}
