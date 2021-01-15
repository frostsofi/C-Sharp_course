using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAppBookShop.Services;
using System.Collections.Generic;
using BookShopCore;
using System;

namespace Tests
{
  [TestClass]
  public class TestBookShopServiceCommon
  {
    private BookShopService _bookShopService;

    [TestInitialize()]
    public void Initialize()
    {
      List<Book> books = new List<Book>() {
         new Book(0, "Peppy long stocking", "Сhildren's literature", new DateTime(2020, 10, 14), 300),
         new Book(1, "War and Peace", "Novel", new DateTime(2020, 10, 10), 500)
       };

      _bookShopService = new BookShopService();
      _bookShopService.SellAll();
      _bookShopService.AcceptBooks(books);
    }

    [TestMethod]
    public void DeleteNonExistentBook()
    {
      Assert.ThrowsException<ArgumentException>(() => _bookShopService.Sell(10));
    }

    [TestMethod]
    public void DeleteExistentBook()
    {
      var books = _bookShopService.GetBooksList();
      Book book = books[0];
      _bookShopService.Sell(books[0].Id);
      bool isExists = _bookShopService.GetBooksList().Contains(book);
      Assert.AreEqual(false, isExists);
    }

    //Проверка, что счет верно увеличивается после удаления
    [TestMethod]
    public void AccountAfterBookDeletion()
    {
      double cost = _bookShopService.GetShopAccount();
      var books = _bookShopService.GetBooksList();
      double costOfBook = books[0].Cost;
      double result = cost + costOfBook;

      _bookShopService.Sell(books[0].Id);
      Assert.AreEqual(result, _bookShopService.GetShopAccount());
    }

    //Проверка, что счет изменяется верно после поставки (поставка возмножна, так как книг мало)
    [TestMethod]
    public void AccontAfterSupply()
    {
      List<Book> books = new List<Book>() {
         new Book(3, "Funny stories", "Stories", new DateTime(2020, 10, 2), 100),
         new Book(4, "Dandelion wine", "Stories", new DateTime(2020, 9, 30), 200)
       };

      double accBefore = _bookShopService.GetShopAccount();
      double resultCost = books[0].Cost + books[1].Cost;
      resultCost += resultCost * 0.07;

      double accAfter = accBefore - resultCost;
      _bookShopService.AcceptBooks(books);
      Assert.AreEqual(accAfter, _bookShopService.GetShopAccount());
    }

    //Книг много и они новые, поэтому поставка не нужна
    [TestMethod]
    public void BadSupplyEnoughBooksInShop()
    {
      List<Book> books = new List<Book>() {
         new Book(5, "Book1", "Stories", DateTime.Now, 1),
         new Book(6, "Book2", "Stories", DateTime.Now, 2),
         new Book(7, "Book3", "Stories", DateTime.Now, 1),
         new Book(8, "Book4", "Stories", DateTime.Now, 2),
         new Book(9, "Book5", "Stories", DateTime.Now, 1),
         new Book(10, "Book5", "Stories", DateTime.Now, 1)
       };

      _bookShopService.AcceptBooks(books);
      Book book = new Book(11, "Book7", "Stories", DateTime.Now, 50);
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.AcceptBooks(new List<Book> { book }));
    }

    //Слишком много старых книг, поставка необходима
    [TestMethod]
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

      Book book = new Book(18, "Book7", "Stories", DateTime.Now, 50);
      try
      {
        _bookShopService.AcceptBooks(new List<Book> { book });
      }
      catch (Exception ex)
      {
        Assert.Fail("Expected no exception, but got: " + ex.Message);
      }
    }

    [TestMethod]
    public void BadSupplyNotEnoughMoney()
    {
      Book book = new Book(21, "Book7", "Stories", DateTime.Now, 2000);
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.AcceptBooks(new List<Book> { book }));
    }
  }

  [TestClass]
  public class BookShopSale
  {
    private BookShopService _bookShopService;

    [TestInitialize()]
    public void SetUp()
    {
      List<Book> books = new List<Book>() {
         new Book(22, "Hobbit", "Fiction", new DateTime(2020, 8, 14), 200),
         new Book(23, "Around the world", "Adventures", new DateTime(2020, 8, 10), 200),
         new Book(24, "The World", "Encyclopedia", new DateTime(2020, 8, 10), 200),
       };

      _bookShopService = new BookShopService();

      _bookShopService.SellAll();
      _bookShopService.AcceptBooks(books);
    }

    [TestMethod]
    public void IsSaleRightDependGenre()
    {
      double startVal = 200;
      double[] expRes = new double[3] { startVal * (1 - 0.03), startVal * (1 - 0.07), startVal * (1 - 0.1) };
      _bookShopService.MakeSale();

      double[] res = new double[3];

      List<Book> books = _bookShopService.GetBooksList();

      for (int i = 0; i < 3; ++i)
      {
        res[i] = books[i].Cost;
      }

      CollectionAssert.AreEqual(expRes, res);
    }

    //Нельзя установить скидку, если она уже установлена
    [TestMethod]
    public void DoubleSale()
    {
      _bookShopService.MakeSale();
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.MakeSale());
    }

    //Цена книги должна быть как и до скидки
    [TestMethod]
    public void RightCancelOfSale()
    {
      _bookShopService.GetSale()._isSaleNow = true;
      _bookShopService.CancelSale();

      double expValue = 200;
      double[] expRes = new double[3] { expValue, expValue, expValue };

      double[] res = new double[3];
      List<Book> books = _bookShopService.GetBooksList();

      for (int i = 0; i < 3; ++i)
      {
        res[i] = books[i].Cost;
      }

      CollectionAssert.AreEqual(expRes, res);
    }

    //Нельзя отменять скидку, если она не была установлена
    [TestMethod]
    public void CancelSaleBeforeStart()
    {
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.CancelSale());
    }

    //Проверка, что на новую скидку не действует скидка
    [TestMethod]
    public void SaleNotForNewBook()
    {
      List<Book> books = _bookShopService.GetBooksList();

      books[0].ReceiptDate = DateTime.Now;
      _bookShopService.MakeSale();

      Assert.AreEqual(200, books[0].Cost);
    }
  }

}
