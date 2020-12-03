using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookShopEntityFramework;
using BookShopSystem;

#warning убрать лишние using'и
namespace WebAppBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SampleFactory _sampleFactory;

        public BooksController(SampleFactory iSampleFactory)
        {
            _sampleFactory = iSampleFactory;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Book>> GetBooks()
        {
            using (var context = _sampleFactory.CreateDbContext(null))
            {
                return await context.GetBooks();
            }
        }

        [HttpGet("{id}", Name = "Get")]
        [Route("{id:int}")]
        public Book GetBook(int iId)
        {
            using (var context = _sampleFactory.CreateDbContext(null))
            {
                return context.GetBook(iId);
            }
        }

        [HttpPost]
        public void Post([FromBody] Book iBook)
        {
            #warning зачем передавать null, если там этот параметр никак не используется? 
            using (var context = _sampleFactory.CreateDbContext(null))
            {
                context.AddBook(iBook);
            }
        }

        [HttpPut("{id}")]
        public void Put(int iId, [FromBody] Book iBook)
        {
            iBook.Id = iId;

            using (var context = _sampleFactory.CreateDbContext(null))
            {
                context.UpdateBook(iBook);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int iId)
        {
            using (var context = _sampleFactory.CreateDbContext(null))
            {
                context.RemoveBook(iId);
            }
        }
    }
}