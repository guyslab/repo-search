using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff] ";
            options.IncludeScopes = false;
        });
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        
        var port = Environment.GetEnvironmentVariable("API_PORT") ?? "5000";
        builder.WebHost.UseUrls($"http://*:{port}");

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationLayer(builder.Configuration);
        builder.Services.AddInfrastructureLayer(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<UserIdMiddleware>();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}    
