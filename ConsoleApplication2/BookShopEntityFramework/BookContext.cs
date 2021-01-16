using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShopCore;

namespace BookShopEntityFrameworkBookConfiguration
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

        public Book GetBook(int iId)
        {
            return Set<Book>().Find(iId);
        }

        public async void RemoveBook(int iId)
        {
            Book book = await Set<Book>().FirstOrDefaultAsync(x => x.Id == iId);
            Set<Book>().Remove(book);
        }
    }
}