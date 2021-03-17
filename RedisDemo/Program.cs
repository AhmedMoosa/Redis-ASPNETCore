using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedisDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var db = provider.GetRequiredService<ApplicationDbContext>();
                
                db.Database.Migrate();

                if (!db.Patients.Any())
                {
                    var patients = Enumerable.Range(1, 10).Select((p, index) => new Patient
                    {
                        BirthDate = DateTime.Parse("1/1/1950"),
                        FirstName = "FirstName_" + index,
                        LastName = "LastName_" + index,
                        Id = Guid.NewGuid().ToString()
                    });
                    await db.Patients.AddRangeAsync(patients);
                    await db.SaveChangesAsync();
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
