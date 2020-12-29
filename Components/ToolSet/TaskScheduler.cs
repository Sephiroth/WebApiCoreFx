using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToolSet
{
    public class TaskScheduler
    {
        private static StdSchedulerFactory factory;

        static TaskScheduler()
        {
            factory = new StdSchedulerFactory();
        }


        public async ValueTask Test()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<MyCustomJob>().WithIdentity("job1", "group1").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1")
                .StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);
            await Task.Delay(TimeSpan.FromSeconds(60));
            await scheduler.Shutdown();
            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

    }

    class ConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }
        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }
        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }
        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }

    public class MyCustomJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            dynamic obj = new { JobName = $"自定义Job:{DateTime.Now}" };
            context.Result = obj;
            await Console.Out.WriteLineAsync(obj.JobName);
        }
    }

}