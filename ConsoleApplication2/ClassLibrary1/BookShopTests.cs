using System;
using System.Collections.Generic;
using NUnit.Framework;
using BookShopSystem;

namespace Tests
{
   [TestFixture]
   public class TestBookShopSystemCommon
   {
     private BookShopSystem.BookShop _bookSystem;

     [SetUp]
     public void SetUp()
     {
       List<Book> books = new List<Book>() {
         new Book(0, "Peppy long stocking", "Сhildren's literature", new DateTime(2020, 10, 14), 300),
         new Book(1, "War and Peace", "Novel", new DateTime(2020, 10, 10), 500) 
       };

      _bookSystem = new BookShopSystem.BookShop(books, 1000);
     }

     [Test]
     public void DeleteNonExistentBook()
     {
       Assert.Throws<ArgumentException>(() =>_bookSystem.Sell(10));
     }

     //проверка, что книги не существует после удаления
     [Test]
     public void DeleteExistentBook()
     {
       var books = _bookSystem.GetBooksList();
       Book book = books[0];
       _bookSystem.Sell(books[0].Id);
       bool isExists = books.Contains(book);
       Assert.AreEqual(false, isExists);
     }

     //Проверка, что счет верно увеличивается после удаления
     [Test]
     public void AccountAfterBookDeletion()
     {
       double cost = _bookSystem.GetShopAccount();
       var books = _bookSystem.GetBooksList();
       double costOfBook = books[0].Cost;
       double result = cost + costOfBook;

       _bookSystem.Sell(books[0].Id);
       Assert.AreEqual(result, _bookSystem.GetShopAccount());
     } 

     //Проверка, что счет изменяется верно после поставки (поставка возмножна, так как книг мало)
     [Test]
     public void AccontAfterSupply()
    {
      List<Book> books = new List<Book>() {
         new Book(3, "Funny stories", "Stories", new DateTime(2020, 10, 2), 100),
         new Book(4, "Dandelion wine", "Stories", new DateTime(2020, 9, 30), 200)
       };

      double accBefore = _bookSystem.GetShopAccount();
      double resultCost = books[0].Cost + books[1].Cost;
      resultCost += resultCost * 0.07;

      double accAfter = accBefore - resultCost;
      _bookSystem.AcceptBooks(books);
      Assert.AreEqual(accAfter, _bookSystem.GetShopAccount());
    }

    //Книг много и они новые, поэтому поставка не нужна
    [Test]
    public void BadSupplyEnoughBooksInShop()
    {
      List<Book> books = new List<Book>() {
         new Book(5, "Book1", "Stories", DateTime.Now, 100),
         new Book(6, "Book2", "Stories", DateTime.Now, 200),
         new Book(7, "Book3", "Stories", DateTime.Now, 100),
         new Book(8, "Book4", "Stories", DateTime.Now, 200),
         new Book(9, "Book5", "Stories", DateTime.Now, 100),
         new Book(10, "Book5", "Stories", DateTime.Now, 100)
       };

      BookShopSystem.BookShop bsSystem = new BookShopSystem.BookShop(books, 1000);
      Book book = new Book(11, "Book7", "Stories", DateTime.Now, 50);
      Assert.Throws<ApplicationException>(() => bsSystem.AcceptBooks(new List<Book>{book}));
    }

    //Слишком много старых книг, поставка необходима
    [Test]
    public void GoodSupplyOldBooksInShop()
    {
      DateTime oldDate = new DateTime(10, 10, 18);

      List<Book> books = new List<Book>() {
         new Book(12, "Book1", "Stories", oldDate, 100),
         new Book(13, "Book2", "Stories", oldDate, 200),
         new Book(14, "Book3", "Stories", oldDate, 100),
         new Book(15, "Book4", "Stories", oldDate, 200),
         new Book(16, "Book5", "Stories", oldDate, 100),
         new Book(17, "Book5", "Stories", oldDate, 100)
       };

      BookShopSystem.BookShop bsSystem = new BookShopSystem.BookShop(books, 1000);
      Book book = new Book(18, "Book7", "Stories", DateTime.Now, 50);
      Assert.DoesNotThrow(() => bsSystem.AcceptBooks(new List<Book> { book }));
    }

    [Test]
    public void BadSupplyNotEnoughMoney()
    {
      List<Book> books = new List<Book>() {
         new Book(19, "Book1", "Stories", new DateTime(2020, 9, 10), 100),
         new Book(20, "Book2", "Stories", new DateTime(2020, 1, 20), 100)
      };

      _bookSystem.AcceptBooks(books);
      Book book = new Book(21, "Book7", "Stories", DateTime.Now, 1500);
      Assert.Throws<ApplicationException>(() => _bookSystem.AcceptBooks(new List<Book>{book}));
    }
  }

  public class TestBookShopSystemSale
  {
    private BookShopSystem.BookShop _bookSystem;
    private List<Book> _books;

    [SetUp]
    public void SetUp()
    {
      List<Book> books = new List<Book>() {
         new Book(22, "Hobbit", "Fiction", new DateTime(2020, 8, 14), 200),
         new Book(23, "Around the world", "Adventures", new DateTime(2020, 8, 10), 200),
         new Book(24, "The World", "Encyclopedia", new DateTime(2020, 8, 10), 200),
       };

      _books = books;
      _bookSystem = new BookShopSystem.BookShop(books, 1000);
    }

    [Test]
    public void IsSaleRightDependGenre()
    {
      double startVal = 200;
      double[] expRes = new double[3]{startVal*(1-0.03), startVal*(1-0.07), startVal*(1-0.1)};
      _bookSystem.MakeSale();

      double[] res = new double[3];

      for(int i = 0; i < 3; ++i)
      {
        res[i] = _books[i].Cost;
      }

      Assert.AreEqual(expRes, res);
    }

    //Нельзя установить скидку, если она уже установлена
    [Test]
    public void DoubleSale()
    {
      _bookSystem.MakeSale();
      Assert.Throws<ApplicationException>(() => _bookSystem.MakeSale());
    }
    
    //Цена книги должна быть как и до скидки
    [Test]
    public void RightCancelOfSale()
    {
      _bookSystem.MakeSale();
      _bookSystem.CancelSale();

      double expValue = 200;
      double[] expRes = new double[3] {expValue, expValue, expValue};
   
      double[] res = new double[3];

      for (int i = 0; i < 3; ++i)
      {
        res[i] = _books[i].Cost;
      }

      Assert.AreEqual(expRes, res);
    }
    
    //Нельзя отменять скидку, если она не была установлена
    [Test]
    public void CancelSaleBeforeStart()
    {
      Assert.Throws<ApplicationException>(() => _bookSystem.CancelSale());
    }

    //Проверка, что на новую скидку не действует скидка
    [Test]
    public void SaleNotForNewBook()
    {
      _books[0].ReceiptDate = DateTime.Now;
      _bookSystem.MakeSale();

      Assert.AreEqual(200, _books[0].Cost);
    }
  }
}
