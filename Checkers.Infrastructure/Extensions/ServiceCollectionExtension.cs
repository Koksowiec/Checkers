using Checkers.Domain.Interfaces;
using Checkers.Infrastructure.Persistance;
using Checkers.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            // DB: https://www.youtube.com/watch?v=S7SdtcIr28s
            service.AddDbContext<CheckersDbContext>(options => options.UseSqlite(
                configuration.GetConnectionString("localDb")));

            service.AddScoped<IGameRepository, GameRepository>();
            service.AddScoped<IMovesRepository, MovesRepository>();
        }
    }
}
