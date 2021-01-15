using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WebAppBookShop.Jobs;

namespace QuartzWithScopedServices
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public QuartzHostedService(IJobFactory iJobFactory)
        {
            _jobFactory = iJobFactory;
        }

        public async Task StartAsync(CancellationToken iCancellationToken)
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.JobFactory = _jobFactory;

            IJobDetail job = JobBuilder.Create<CheckingEnoughBooks>().Build();
            ITrigger trigger = TriggerBuilder
                .Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(2)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(job, trigger, iCancellationToken);
            await _scheduler.Start(iCancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}