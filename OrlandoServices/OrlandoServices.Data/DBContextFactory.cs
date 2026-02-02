using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data
{
    public class DBContextFactory : IDesignTimeDbContextFactory<DBContext>
    {
            public DBContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
                optionsBuilder.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")
                );

                return new DBContext(optionsBuilder.Options);
            }
        }
    }

