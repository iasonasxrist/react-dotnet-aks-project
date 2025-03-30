﻿namespace Movies.Contracts.Responses;

public class MovieResponse
{
    public required Guid Id { get; init; }
    
    public required string Title { get; init; }
    
}
