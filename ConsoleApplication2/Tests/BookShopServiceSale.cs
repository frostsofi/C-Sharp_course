using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAppBookShop.Services;
using System.Collections.Generic;
using BookShopCore;
using System;

namespace Tests
{
  [TestClass]
  class UnitTest1
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

      _bookShopService.AcceptBooks(books);
      _bookShopService = new BookShopService();

      _bookShopService.SellAll();
      _bookShopService.AcceptBooks(books);
    }

    [TestInitialize()]
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

      Assert.AreEqual(expRes, res);
    }

    //Нельзя установить скидку, если она уже установлена
    [TestInitialize()]
    public void DoubleSale()
    {
      _bookShopService.MakeSale();
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.MakeSale());
    }

    //Цена книги должна быть как и до скидки
    [TestInitialize()]
    public void RightCancelOfSale()
    {
      _bookShopService.MakeSale();
      _bookShopService.CancelSale();

      double expValue = 200;
      double[] expRes = new double[3] { expValue, expValue, expValue };

      double[] res = new double[3];
      List<Book> books = _bookShopService.GetBooksList();

      for (int i = 0; i < 3; ++i)
      {
        res[i] = books[i].Cost;
      }

      Assert.AreEqual(expRes, res);
    }

    //Нельзя отменять скидку, если она не была установлена
    [TestInitialize()]
    public void CancelSaleBeforeStart()
    {
      Assert.ThrowsException<ApplicationException>(() => _bookShopService.CancelSale());
    }

    //Проверка, что на новую скидку не действует скидка
    [TestInitialize()]
    public void SaleNotForNewBook()
    {
      List<Book> books = _bookShopService.GetBooksList();

      books[0].ReceiptDate = DateTime.Now;
      _bookShopService.MakeSale();

      Assert.AreEqual(200, books[0].Cost);
    }
  }
}
