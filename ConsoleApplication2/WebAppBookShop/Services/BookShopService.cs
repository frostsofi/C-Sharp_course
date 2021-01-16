using System;
using System.Collections.Generic;
using BookShopCore;
using BookShopEntityFramework;

namespace WebAppBookShop.Services
{
    public class BookShopService
    {
        private readonly SampleFactory _sampleFactory;
        private readonly int _maxCountBooks = 50;
        private double _shopScore = 1800;
        private Sale _sale;

        public class Sale
        {
            public bool _isSaleNow { get; set; }
            public DateTime _startDate { get; set; }

            public Sale()
            {
                _isSaleNow = false;
            }
        }

        public BookShopService()
        {
            _sampleFactory = new SampleFactory();
            _sale = new Sale();
        }

        public List<Book> GetBooksList()
        {
            using (var context = _sampleFactory.CreateDbContext())
            {
                return context.GetBooks().Result;
            }
        }

        public double GetShopAccount()
        {
            return _shopScore;
        }

        public Sale GetSale()
        {
            return _sale;
        }

        //Need for unit-tests
        public void SellAll()
        {
            using (var context = _sampleFactory.CreateDbContext())
            {
                context.RemoveRange(context.GetBooks().Result);
                context.SaveChanges();
            }
        }

        public void Sell(int iGuidOfBook)
        {
            using (var context = _sampleFactory.CreateDbContext())
            {
                if (context.GetBook(iGuidOfBook) == null)
                    throw new ArgumentException("This book does not exist");

                _shopScore += context.GetBook(iGuidOfBook).Cost;
                context.RemoveBook(iGuidOfBook);
            }
        }

        private void IsEnoughMoney(double iResCost)
        {
            if (iResCost > _shopScore)
                throw new ApplicationException("Not enough money on shop account");
        }

        private bool IsTooManyOldBooks()
        {
            bool isTooMany = false;
            int emountOldBooks = 0;

            using (var context = _sampleFactory.CreateDbContext())
            {
                List<Book> books = context.GetBooks().Result;
                foreach (var book in books)
                {
                    DateTime rDate = book.ReceiptDate;
                    TimeSpan dateDiff = DateTime.Now.Subtract(rDate);
                    if (dateDiff.Days > 30)
                        emountOldBooks++;
                }

                if (emountOldBooks >= books.Count * 0.75)
                    isTooMany = true;
            }

            return isTooMany;
        }

        private bool IsTooFewBooksInShop()
        {
            int emountOfBooks = 0;
            using (var context = _sampleFactory.CreateDbContext())
            {
                emountOfBooks = context.GetBooks().Result.Count;
            }

            if (emountOfBooks < 0.1 * _maxCountBooks)
                return true;
            return false;
        }

        public bool IsNeededAcceptance()
        {
      if (!(IsTooFewBooksInShop() || IsTooManyOldBooks()))
        throw new ApplicationException("New arrival of books is not needed");
      else
        return true;
        }

        private double TotalCost(List<Book> iNewBooks)
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
            double sumCost = TotalCost(iNewBooks);
            double debt = 0.07 * sumCost;
            double resCost = sumCost + debt;

            IsEnoughMoney(resCost);

            using (var context = _sampleFactory.CreateDbContext())
            {
                context.AddRange(iNewBooks);
                context.SaveChanges();
            }

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

            using (var context = _sampleFactory.CreateDbContext())
            {
                List<Book> books = context.GetBooks().Result;

                foreach (var book in books)
                {
                    if (IsNewBook(book))
                    {
                        continue;
                    }

                    double newCost = book.Cost * GetFactor(book.Genre);
                    book.Cost = newCost;
                }

                context.SaveChanges();
            }
        }

        public void CancelSale()
        {
            if (!_sale._isSaleNow)
                throw new ApplicationException("Sale was not scheduled");

            _sale._isSaleNow = false;

            using (var context = _sampleFactory.CreateDbContext())
            {
                List<Book> books = context.GetBooks().Result;

                foreach (var book in books)
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
    }
}