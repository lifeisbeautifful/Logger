using LogComponent;

namespace LogUsers
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            
            var loggerService = new LoggerService(new AsyncLog());
            Console.WriteLine("Before que");

            ThreadPool.QueueUserWorkItem(new WaitCallback(item => loggerService.AddLogLines("Number with Flush: ", 150)));
            Console.WriteLine("After que");

            var loggerService2 = new LoggerService(new AsyncLog());
            ThreadPool.QueueUserWorkItem(new WaitCallback(item => loggerService2.AddLogLines("Number without Flush: ", 150)));

            Thread.Sleep(1000);
            loggerService.StopLoggingWithFlush(true);
            loggerService2.StopLoggingWithFlush(false);

            Console.ReadLine();
        }
    }
}
