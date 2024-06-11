using E_Commerce.Apis.Errors;
using E_Commerce.Apis.Extensions;
using E_Commerce.Apis.Helpers;
using E_Commerce.Apis.MiddleWares;
using E_Commerce.Core.Entities.Identity_Module;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Repository.Data.Contexts;
using E_Commerce.Repository.Identity;
using E_Commerce.Repository.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace E_Commerce.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            #region Configure Services(Add services to the container)

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });//allow Dependence injection

            //connection with Radis
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("RadisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            


            //call for ApplicationServicesExtension Class
            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", o =>
                {
                    o.AllowAnyHeader();
                    o.AllowAnyMethod();
                    o.WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });
           
                
           
           
           

            #endregion


            var app = builder.Build();


            #region Update-Database

           using var scope=app.Services.CreateScope();
            //container for ScopedServices (Group of Services LifeTime Scoped)
            var services=scope.ServiceProvider;

            //Services Its Self

            var LoggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();

                if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    //Ask Clr for creating object from dbContext Explicitly
                    await dbContext.Database.MigrateAsync();
                }

                //Apply Seeding if Db is Empty
                await DataContextSeed.SeedAsync(dbContext);

                var     IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();

                if ((await IdentityDbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    //Ask Clr for creating object from dbContext Explicitly
                    await IdentityDbContext.Database.MigrateAsync();
                }
                var userManager=services.GetRequiredService<UserManager<AppUser>>();
                //Apply Seeding if Db is Empty
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);





            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex,"An Error OCCured During Appling The Migration");
             
            }


            
            #endregion


            #region Configure MiddleWare(Configure the HTTP request pipeline)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
                app.UseMiddleware<ExceptionMiddleWare>();
            }
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
