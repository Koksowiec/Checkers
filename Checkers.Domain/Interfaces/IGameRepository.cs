using Checkers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task Commit();
        Task CreateAsync(Game game);
        Task<IEnumerable<Game>> GetAllAsync();
    }
}
