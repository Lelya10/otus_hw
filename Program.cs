using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MyService.BD;

namespace MyService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        string connection = builder.Configuration.GetConnectionString("DefaultConnection") + 
                            "Username=" + Environment.GetEnvironmentVariable("USERNAME") + ";Password="+
                            Environment.GetEnvironmentVariable("PASSWORD");
 
        // добавляем контекст ApplicationContext в качестве сервиса в приложение
        Console.WriteLine(connection);
        builder.Services.AddDbContext<BDManage>(options => options.UseNpgsql(connection));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
 
        app.MapGet("/health", (HttpContext httpContext) => new StatusCode()
            {
                Status = "OK"
            })
            .WithName("GetHealthCheck")
            .WithOpenApi();
        
        app.MapGet("/user/all", (BDManage db) => db.Users.ToList());
        app.MapGet("/user/{id}", (BDManage db, long id) => db.GetUser(id));
        app.MapDelete("/user/{id}", (BDManage db, long id) => db.DeleteUser(id));
        app.MapPost("user/add", (BDManage db, AddUser user) => db.AddUser(user));
        app.MapPut("user/update", (BDManage db, User user) => db.UpdateUser(user));
        
        
        app.Run();
    }
}