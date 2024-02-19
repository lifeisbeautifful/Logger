using LogComponent.Abstract;
using LogComponent.Implementation;
using Microsoft.Extensions.DependencyInjection;


namespace LogUsers
{
  
    class Program
    {
        static void Main(string[] args)
        {

            var asyncLogForIncreaseLoop = new AsyncLoggerCreator().CreateLogger();
            asyncLogForIncreaseLoop.StartLogging();


            //Satisfy demands 1: execute write call asynchroniously,
            //so application can get on with its work without waiting for the log to be written to a file.
            ThreadPool.QueueUserWorkItem(item => asyncLogForIncreaseLoop.AddLinesIncreaseCount("Number with Flush: "));

            
            var asyncLogForDecreaseLoop = new AsyncLoggerCreator().CreateLogger();
            asyncLogForDecreaseLoop.StartLogging();

            // Satisfy demands 1: execute write call asynchroniously, use if we
            //don`t need to start logging immediately.
            var startLog = new Task(() => asyncLogForDecreaseLoop.AddLinesDecreaseCount("Number with No flush: "));
            startLog.Start();


            Thread.Sleep(1000);
            asyncLogForIncreaseLoop.StopWithoutFlush();
            asyncLogForDecreaseLoop.StopWithFlush();
            Console.ReadLine();
        }
    }
}
