using Checkers.Domain.Interfaces;
using Checkers.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Infrastructure.Repositories
{
    public class GameDetailsRepository : IGameDetailsRepository
    {
        private readonly CheckersDbContext _dbContext;

        public GameDetailsRepository(CheckersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
