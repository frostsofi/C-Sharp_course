using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookShopSystem
{
  public class Book
  {
    [JsonProperty("id")]
    public int Id {get; set;}

    [JsonProperty("title")]
    public string Name {get; set;}

    [JsonProperty("genre")]
    public string Genre { get; set;}

    [JsonProperty("dateOfDelivery")]
    public DateTime ReceiptDate { get; set;}

    [JsonProperty("price")]
    public double Cost { get; set;}

    public Book()
    {
    }

    public Book(int iGuid, string iName, string iGenre, DateTime iDate, double iCost)
    {
      Id = iGuid;
      Name = iName;
      Genre = iGenre;
      ReceiptDate = iDate;
      Cost = iCost;
    }
  }

  public class BookShop
  {
    private int _maxCountBooks = 50;
    private double _shopScore;
    private List<Book> _currentBooks;
    private Sale _sale;

    private class Sale
    {
      public bool _isSaleNow{get; set;}
      public DateTime _startDate { get; set; }

      public Sale()
      {
        _isSaleNow = false;
      }
    }

    public BookShop(List<Book> iBooks, double iStartScore)
    {
      _currentBooks = iBooks;
      _shopScore = iStartScore;
      _sale = new Sale();
    }

    public List<Book> GetBooksList()
    {
      return _currentBooks;
    }

    public double GetShopAccount()
    {
      return _shopScore;
    }

    public void Sell(int iGuidOfBook)
    {
      int index = _currentBooks.FindIndex(Book => Book.Id.Equals(iGuidOfBook));

      if (index == -1)
        throw new ArgumentException("This book does not exist");

      Book bookToSale = _currentBooks[index];

      double costOfBook = bookToSale.Cost;
      _shopScore += costOfBook;
      _currentBooks.Remove(bookToSale);
    }

    private void IsEnoughMoney(double iResCost)
    {
      if (iResCost > _shopScore)
        throw new ApplicationException("Not enough money on shop account");
    }

    private bool IsTooManyOldBooks()
    {
      int emountOldBooks = 0;

      foreach(var book in _currentBooks)
      {
        DateTime rDate = book.ReceiptDate;
        TimeSpan dateDiff = DateTime.Now.Subtract(rDate);
        if (dateDiff.Days > 30)
          emountOldBooks++;
      }

      if (emountOldBooks >= _currentBooks.Count * 0.75)
        return true;

      return false;
    }

    private bool IsTooFewBooksInShop()
    {
      int emountOfBooks = _currentBooks.Count;
      if (emountOfBooks < 0.1 * _maxCountBooks)
        return true;
      return false;
    }

    private void IsNeededAcceptance()
    {
      if (!(IsTooFewBooksInShop() || IsTooManyOldBooks()))
        throw new ApplicationException("New arrival of books is not needed");
    }

    private double totalCost(List<Book> iNewBooks)
    {
      double sum = 0.0;
      foreach (var book in iNewBooks)
      {
        sum += book.Cost;
      }

      return sum;
    } 

    public void AcceptBooks(List<Book> iNewBooks)
    {
      IsNeededAcceptance();
      double sumCost = totalCost(iNewBooks);
      double debt = 0.07 * sumCost;
      double resCost = sumCost + debt;

      IsEnoughMoney(resCost);
      _currentBooks.Union(iNewBooks);
      _shopScore -= resCost;
    } 

    private bool IsNewBook(Book iBook)
    {
      TimeSpan diffDates = _sale._startDate.Subtract(iBook.ReceiptDate);
      if (diffDates.Days < 14)
        return true;

      return false;
    }

    private double GetFactor(string iGenre)
    {
      double fictionSale = 0.03;
      double adventureSale = 0.07;
      double encyclopediaSale = 0.1;

      double factor = 1.0;
      switch (iGenre)
      {
        case "Fiction":
          factor -= fictionSale;
          break;
        case "Adventures":
          factor -= adventureSale;
          break;
        case "Encyclopedia":
          factor -= encyclopediaSale;
          break;
      }
      return factor;
    }

    public void MakeSale()
    {
      if (_sale._isSaleNow)
        throw new ApplicationException("Sale already scheduled");

      _sale._isSaleNow = true;
      _sale._startDate = DateTime.Now;
     
      foreach(var book in _currentBooks)
      {
        if (IsNewBook(book))
        {
          continue;
        }

        double newCost = book.Cost * GetFactor(book.Genre);
        book.Cost = newCost;
      }
    }

    public void CancelSale()
    {
      if (!_sale._isSaleNow)
        throw new ApplicationException("Sale was not scheduled");

      _sale._isSaleNow = false;

      foreach (var book in _currentBooks)
      {
        if (IsNewBook(book))
        {
          continue;
        }

        double newCost = book.Cost / GetFactor(book.Genre);
        book.Cost = newCost;
      }
    } 
  }
  
  class Program
  {
    static void Main(string[] args)
    {
    }
  }
}
