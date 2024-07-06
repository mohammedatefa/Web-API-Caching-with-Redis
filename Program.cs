using Caching_with_Redis.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Caching_with_Redis.Utalitis.UOW;
using Caching_with_Redis.Repositories;
using Caching_with_Redis.Services;
namespace Caching_with_Redis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //connection string configurations
            builder.Services.AddDbContext<SalseManagementDbContext>(options =>
            {
                options.UseSqlServer(builder
                    .Configuration.GetConnectionString("SalesManagementConnectionString"));
            });

            //inject Redis Service
            builder.Services.AddScoped(typeof(ICachingService), typeof(CachingService));

            //Inject Reposiotries
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //injetct AutoMapper service 
            builder.Services.AddAutoMapper(typeof(Program));

            //Inject FluentValidation 
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
