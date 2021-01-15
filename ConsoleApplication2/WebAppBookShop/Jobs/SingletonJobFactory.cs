using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace WebAppBookShop.Jobs
{
  public class SingletonJobFactory : IJobFactory
  {
    private readonly IServiceProvider _serviceProvider;
    public SingletonJobFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      return _serviceProvider.GetRequiredService<CheckingEnoughBooks>();
    }

    public void ReturnJob(IJob job)
    {
    }
  }
}
