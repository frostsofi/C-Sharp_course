using System.Collections.Generic;
using BookShopContract;

namespace WebBooksOrder.MassTransit
{
  public class SendBooksContract : SendBooks
  {
    public List<Book> _books { get; set; }
  }
}
