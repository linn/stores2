﻿namespace Linn.Stores2.Domain.LinnApps.Tests
{
    using System;

    using Linn.Stores2.Persistence.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class TestServiceDbContext : ServiceDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}
