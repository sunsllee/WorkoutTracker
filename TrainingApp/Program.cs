using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;

namespace TrainingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TrainingAppContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueApp",
                    policy =>
                    {
                        policy.WithOrigins(
                                "http://localhost:8080",    
                                "https://localhost:8080",   
                                "http://localhost:5173",    
                                "https://localhost:5173"
                            )
                            .AllowAnyHeader()               
                            .AllowAnyMethod()               
                            .AllowCredentials();           
                    });
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowVueApp");  


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}