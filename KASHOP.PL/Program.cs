
using System;
using KASHOP.BLL;
using KASHOP.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

using Microsoft.Extensions.Options;
using KASHOP.DAL.Repository;
using KASHOP.BLL.Service;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using KASHOP.DAL.Utils;




namespace KASHOP.PL;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.


        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddLocalization(options => options.ResourcesPath = "");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

        builder.Services.AddIdentity<ApplicationUser , IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        const string defaultCulture = "en";
        var supportedCultures = new[]
        {
            new CultureInfo(defaultCulture),
            new CultureInfo("ar")
        };
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
            {
                QueryStringKey = "lang",

            });
        });
        //
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService , CategoryService>();
        builder.Services.AddScoped<ISeedData , RoleSeedData>();
        builder.Services.AddScoped<ISeedData , UserSeedData>();
        builder.Services.AddScoped<IAuthenticationService , AuthenticationService>();

        var app = builder.Build();
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        using (var Scope = app.Services.CreateScope())
        {
            var services = Scope.ServiceProvider;
            var seeders = services.GetServices<ISeedData>();
            foreach (var seeder in seeders)
            {
               await seeder.DataSeed();
            }

        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
