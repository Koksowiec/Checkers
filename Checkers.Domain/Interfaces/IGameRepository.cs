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
        Task CreateAsync(string gameId, string p1Name = "");
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game>? GetGameById(string gameId);
        Task UpdateGameP1(string gameId, string name);
        Task UpdateGameP2(string gameId, string name);
        Task<List<string>> GetDasboard();
        Task UpdateWinner(string gameId, string name);
    }
}
