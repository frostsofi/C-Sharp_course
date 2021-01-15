using System.Collections.Generic;

namespace BookShopContract
{
  public interface SendBooks
  {
    List<Book> _books { get; set; }
  }
}