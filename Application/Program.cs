using LogComponent;

namespace LogUsers
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddScoped<ILoggerService, LoggerService>();
            //serviceCollection.AddScoped<ILog, AsyncLog>();

            //var serviceProvider = serviceCollection.BuildServiceProvider();
            //var loggerService = serviceProvider.GetRequiredService<ILoggerService>();
            
            var loggerService = new LoggerService(new AsyncLog());
            
            
            Console.WriteLine("Before que");

            ThreadPool.QueueUserWorkItem(new WaitCallback(item => loggerService.AddLogLines("Number with Flush: ", 105)));
            Console.WriteLine("After que");

            //var loggerService2 = new LoggerService(new AsyncLog());
            //ThreadPool.QueueUserWorkItem(new WaitCallback(item => loggerService2.AddLogLines("Line ", 50)));

            
            loggerService.StopLogging(true);
            //loggerService2.StopLogging(true);

            //ILog logger2 = new AsyncLog();

            //for (int i = 50; i > 0; i--)
            //{
            //    logger2.Write("Number with No flush: " + i);
            //    Thread.Sleep(20);
            //}

            //logger2.StopWithoutFlush();

            Console.ReadLine();
        }
    }
}
