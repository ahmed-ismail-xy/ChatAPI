using ChatAPI.API.Middlewares;
using ChatAPI.Application;
using ChatAPI.Application.Hubs;
using ChatAPI.Persistence;
using Serilog;

namespace ChatAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);


            // Add services to the container.
            builder.Services.AddAPIContainer(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddControllers();
            builder.Services.AddSingleton<RequestStatusCodeMiddleware>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseMiddleware(typeof(RequestStatusCodeMiddleware));
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapHub<ChatHub>("/chathub");
            app.MapControllers();

            app.Run();
        }
    }
}