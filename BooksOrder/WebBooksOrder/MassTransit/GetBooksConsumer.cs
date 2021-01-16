using System.Threading.Tasks;
using MassTransit;
using BookShopContract;
using Service;
using System.Collections.Generic;
using System.Net.Http;

namespace WebBooksOrder.MassTransit
{
    public class GetBooksConsumer : IConsumer<GetBooks>
    {
        private readonly SendBooksProducer _producer;

        public GetBooksConsumer(SendBooksProducer iProducer)
        {
            _producer = iProducer;
        }

        public async Task Consume(ConsumeContext<GetBooks> context)
        {
          
            var message = context.Message;
            int numberBooks = message._numberBooks;
            ServiceProxy proxy = new ServiceProxy(new HttpClient());
            #warning лучше использовать await
            var task = proxy.GetData<List<BookShopContract.Book>>("https://getbooksrestapi.azurewebsites.net/api/books/" + numberBooks);

            var books = await task;

            #warning лучше использовать await
            await _producer.SentBooks(books);
        }
    }
}