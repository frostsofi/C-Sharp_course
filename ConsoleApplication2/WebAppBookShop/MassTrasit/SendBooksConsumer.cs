using BookShopContract;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Book = BookShopCore.Book;

namespace WebAppBookShop.MassTrasit
{
  public class SendBooksConsumer : IConsumer<SendBooks>
  {
    public Task Consume(ConsumeContext<SendBooks> iContext)
    {
      var message = iContext.Message;
      var books = message._books;
      List<Book> booksBD = new List<Book>();

      foreach (var book in books)
      {
        booksBD.Add(new Book(book.Id, book.Name, book.Genre, book.ReceiptDate, book.Cost));
      }

      /* Can't inject a dependency and now received books just print in console */
      foreach (var book in booksBD)
      {
        Console.WriteLine("Book{0}: Name:{1} Cost:{2}", book.Id, book.Name, book.Cost);
      }

      return Task.CompletedTask;
    }
  }
}
