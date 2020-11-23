using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BookShopEntityFramework
{
  public class SampleFactory : IDesignTimeDbContextFactory<BookContext>
  {
    public BookContext CreateDbContext(string[] args)
    {
      var builder = new ConfigurationBuilder();
      builder.SetBasePath(Directory.GetCurrentDirectory());
      builder.AddJsonFile("DBsettings.json");
      var config = builder.Build();
      string connectionString = config.GetConnectionString("DefaultConnection");

      var optionsBuilder = new DbContextOptionsBuilder<BookContext>();
      var options = optionsBuilder
        .UseNpgsql(connectionString)
        .Options;
      return new BookContext(options);
    }
  }
}