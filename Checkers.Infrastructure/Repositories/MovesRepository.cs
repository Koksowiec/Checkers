using Checkers.Domain.Interfaces;
using Checkers.Infrastructure.Persistance;
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
    }
}
