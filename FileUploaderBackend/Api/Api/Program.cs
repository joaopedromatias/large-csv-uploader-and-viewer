using IoC;
using Serilog;

namespace Api;

public class Program 
{ 
    public static void Main(string[] args) 
    { 
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMemoryCache(); 

        var corsPolicy = "AllowAll";
        builder.Services.AddCors(options => 
        { 
            options.AddPolicy(corsPolicy, policy => 
            { 
                policy.AllowAnyOrigin().WithMethods("GET", "POST").AllowAnyHeader();
            });
        });

        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddClients(builder.Configuration);
        builder.Services.AddLogging(builder);
        builder.Services.AddData(builder.Configuration);        
        builder.Services.AddBackgroundJobs(builder.Configuration);

        builder.Services.AddControllers();

        var app = builder.Build();        

        app.UseCors(corsPolicy);
        
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            { 
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.Run();
    }
}