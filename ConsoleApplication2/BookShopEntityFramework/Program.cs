using System;
using System.Linq;
using BookShopEntityFramework;
using BookShopSystem;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Program
{
  public class Program
  {
    public static void Main(string[] args)
    {

      SampleFactory factory = new SampleFactory();
      using (BookContext db = factory.CreateDbContext(null))
      {
        Book book1 = new Book(3, "Funny stories", "Stories", new DateTime(2020, 10, 2), 100);
        Book book2 = new Book(4, "Dandelion wine", "Stories", new DateTime(2020, 9, 30), 200);

        // добавляем их в бд
        db.AddBook(book1);
        db.AddBook(book2);
        db.SaveChanges();
        Console.WriteLine("Объекты успешно сохранены");

        // получаем объекты из бд и выводим на консоль
        var books = db.GetBooks().GetAwaiter().GetResult();
      
        Console.WriteLine("Список объектов:");
        foreach (Book b in books)
        {
          Console.WriteLine($"{b.Id}.{b.Name} - {b.Genre}");
        } 
      }
    }
  }
}
