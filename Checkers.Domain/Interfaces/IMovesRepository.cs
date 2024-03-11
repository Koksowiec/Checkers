using Checkers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Domain.Interfaces
{
    public interface IMovesRepository
    {
        Task CreateAsync(string gameId);
        Task<Moves>? GetMovesById(string gameId);
        Task UpdateP1Moves(string gameId, string moves);
        Task UpdateP2Moves(string gameId, string moves);
    }
}
