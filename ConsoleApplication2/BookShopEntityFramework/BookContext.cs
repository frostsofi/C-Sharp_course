using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShopSystem;
using System;

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

    public void AddBook(Book iBook)
    {
      Set<Book>().Add(iBook);
    }

    public async Task<List<Book>> GetBooks()
    {
      return await Set<Book>().ToListAsync();
    }

    public Book GetBook(int iId)
    {
      return Set<Book>().Find(iId);
    }

    public void RemoveBook(int iId)
    {
      Book book = Set<Book>().FirstOrDefaultAsync(x => x.Id == iId).GetAwaiter().GetResult();

      Set<Book>().Remove(book);
    }

    public bool UpdateBook(Book iBook)
    {
      if (iBook == null)
      {
        throw new ArgumentNullException("Book is null");
      }
      Book book = Set<Book>().FirstOrDefaultAsync(x => x.Id == iBook.Id).GetAwaiter().GetResult();

      Set<Book>().Remove(book);
      Set<Book>().Add(iBook);
      return true;
    }
  }
}
