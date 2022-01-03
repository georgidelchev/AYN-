﻿using System;
using System.Threading.Tasks;

using AYN.Data.Common;

using Microsoft.EntityFrameworkCore;

namespace AYN.Data;

public class DbQueryRunner : IDbQueryRunner
{
    public DbQueryRunner(ApplicationDbContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ApplicationDbContext Context { get; set; }

    public Task RunQueryAsync(string query, params object[] parameters)
        => this.Context.Database.ExecuteSqlRawAsync(query, parameters);

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context?.Dispose();
        }
    }
}
