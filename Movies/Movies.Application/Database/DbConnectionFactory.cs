using Dapper;

namespace Movies.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                          IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='movies' AND xtype='U')
                                          CREATE TABLE movies (
                                              id UNIQUEIDENTIFIER PRIMARY KEY,
                                              slug NVARCHAR(255) NOT NULL, 
                                              title NVARCHAR(255) NOT NULL,
                                              yearofrelease INT NOT NULL
                                          );
                                      """);

        await connection.ExecuteAsync("""
                                          IF NOT EXISTS (
                                              SELECT * FROM sys.indexes 
                                              WHERE name = 'movies_slug_idx' AND object_id = OBJECT_ID('movies')
                                          )
                                          CREATE UNIQUE INDEX movies_slug_idx ON movies (slug);
                                      """);

        await connection.ExecuteAsync("""
                                          IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='genres' AND xtype='U')
                                          CREATE TABLE genres (
                                              movieId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES movies(id),
                                              name NVARCHAR(255) NOT NULL
                                          );
                                      """);
    }
}