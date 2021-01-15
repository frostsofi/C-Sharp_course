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

        public Task Consume(ConsumeContext<GetBooks> context)
        {
            var message = context.Message;
            int numberBooks = message._numberBooks;
            ServiceProxy proxy = new ServiceProxy(new HttpClient());
#warning лучше использовать await
            var task = proxy.GetData<List<BookShopContract.Book>>("https://getbooksrestapi.azurewebsites.net/api/books/" + numberBooks);

            task.Wait();
            var books = task.Result;

#warning лучше использовать await
            var taskSended = _producer.SentBooks(books);
            taskSended.Wait();

            return Task.CompletedTask;
        }
    }
}