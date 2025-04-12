﻿using System.Data;
using Microsoft.Data.SqlClient;

namespace Movies.Application.Database;

public  interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
        }
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}