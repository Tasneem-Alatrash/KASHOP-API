
using KASHOP.BLL;
using KASHOP.DAL;
using Microsoft.EntityFrameworkCore;
namespace KASHOP.PL;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        Class1 n  = new Class1();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        // builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        //     builder.Configuration["ConnectionStrings:DefaultConnection"]
        // ));
        //  builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        //    builder.Configuration.GetSection(("ConnectionStrings")["DefaultConnection"])
        // ));

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
