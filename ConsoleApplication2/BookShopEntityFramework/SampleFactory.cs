﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookShopEntityFramework
{
  public class SampleFactory : IDesignTimeDbContextFactory<BookContext>
  {
    public BookContext CreateDbContext(string[] args = null)
    {
      var builder = new ConfigurationBuilder();
      builder.SetBasePath(Directory.GetCurrentDirectory());

      builder.AddJsonFile("appsettings.json");
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