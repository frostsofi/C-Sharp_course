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
        #warning всегда-всегда запрашиваются книги? должны же запрашиваться при каком-то условии 
        #warning (что-то в духе если осталось меньше 10% от общей вместимости магазина)
        var producer = scope.ServiceProvider.GetService<GetBooksProducer>();
        await producer.SentNumberOfBooks();
      }

      Console.WriteLine("Job success!");
    }
  }
}

