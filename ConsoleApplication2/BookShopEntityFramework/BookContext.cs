using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShopSystem;

namespace BookShopEntityFramework
{
public class BookContext : DbContext
  {
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new BookConfiguration());
    }

    public async Task<List<Book>> GetBooks()
    {
      return await Set<Book>().ToListAsync();
    }
  }
}
