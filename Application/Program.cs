using LogComponent.Abstract;
using LogComponent.Implementation;
using Microsoft.Extensions.DependencyInjection;


namespace LogUsers
{
  
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddTransient<ILoggerCreator, AsyncLoggerCreator>();
            collection.AddTransient<ILog, AsyncLog>();
            collection.AddSingleton<DateProvider, DateProvider>();

            var serviceProvider = collection.BuildServiceProvider();

            var asyncLogForIncreaseLoop = serviceProvider.GetRequiredService<ILoggerCreator>()
                                                         .CreateLogger(serviceProvider.GetRequiredService<DateProvider>());                                     
            asyncLogForIncreaseLoop.StartLogging();


            //Satisfy demands 1: execute write call asynchroniously,
            //so application can get on with its work without waiting for the log to be written to a file.
            ThreadPool.QueueUserWorkItem(item => asyncLogForIncreaseLoop.AddLinesIncreaseCount("Number with Flush: "));


            var asyncLogForDecreaseLoop = serviceProvider.GetRequiredService<ILoggerCreator>()
                                                         .CreateLogger(serviceProvider.GetRequiredService<DateProvider>());
            asyncLogForDecreaseLoop.StartLogging();

            // Satisfy demands 1: execute write call asynchroniously, use if we
            //don`t need to start logging immediately.
            var startLog = new Task(() => asyncLogForDecreaseLoop.AddLinesDecreaseCount("Number with No flush: ", 400));
            startLog.Start();

            
            //Thread.Sleep(1000);
            asyncLogForIncreaseLoop.StopWithoutFlush();
            asyncLogForDecreaseLoop.StopWithFlush();
            //Console.ReadLine();
        }
    }
}
