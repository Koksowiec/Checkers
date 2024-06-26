﻿using Checkers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Checkers.Infrastructure.Persistance
{
    public class CheckersDbContext : DbContext
    {
        public CheckersDbContext(DbContextOptions<CheckersDbContext> options) : base(options)
        {

        }

        public void PerformCheckpoint()
        {
            Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(FULL);");
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Moves> Moves { get; set; }
    }
}
