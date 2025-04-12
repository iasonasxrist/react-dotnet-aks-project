using Microsoft.EntityFrameworkCore;
using Movies.Api.Mapping;
using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 1. Add CORS services and configure the policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000", //frontend 
                    "http://localhost:8081",
                    "http://localhost:8080",
                    "http://your-other-dev-machine:port",
                    "https://www.your-production-frontend.com"
                )
                .AllowAnyHeader() // Allows common headers
                .AllowAnyMethod() // Allows common HTTP methods (GET, POST, PUT, DELETE, etc.)
                .AllowCredentials(); // *** Include ONLY if your frontend sends cookies or Auth headers ***
            // *** If you add this, you CANNOT use AllowAnyOrigin() ***
        });
});

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config.GetConnectionString("MoviesDb"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();




app.Run();