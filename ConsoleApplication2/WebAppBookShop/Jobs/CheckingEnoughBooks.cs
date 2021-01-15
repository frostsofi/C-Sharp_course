using System;
using System.Threading.Tasks;
using Quartz;
using WebAppBookShop.MassTrasit;
using Microsoft.Extensions.DependencyInjection;

namespace WebAppBookShop.Jobs
{
  public class CheckingEnoughBooks : IJob
  {
    private readonly IServiceProvider _serviceProvider;

    public CheckingEnoughBooks(IServiceProvider iProvider)
    {
      _serviceProvider = iProvider;
    }

    public async Task Execute(IJobExecutionContext iContext)
    {
      using (var scope = _serviceProvider.CreateScope())
      {
        var producer = scope.ServiceProvider.GetService<GetBooksProducer>();
        await producer.SentNumberOfBooks();
      }

      Console.WriteLine("Job success!");
    }
  }
}

