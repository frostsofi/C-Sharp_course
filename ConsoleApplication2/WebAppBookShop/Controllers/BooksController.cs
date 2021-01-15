using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BookShopCore;
using WebAppBookShop.Services;

namespace WebAppBookShop.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase
  {
    private readonly BookShopService _bookShopService;

    public BooksController(BookShopService iBookShopService)
    {
      _bookShopService = iBookShopService;
    }

    [HttpGet]
    public List<Book> GetBooks()
    {
      return _bookShopService.GetBooksList();
    }

    [HttpPost]
    public void Post([FromBody] Book iBook)
    {
      _bookShopService.AcceptBooks(new List<Book> { iBook });
    }

    [HttpDelete("{id}")]
    public void Delete(int iId)
    {
      _bookShopService.Sell(iId);
    }
  }
}
