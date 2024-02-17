using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent
{
    public class LoggerService: ILoggerService
    {
        private ILog _logger;
        public LoggerService(ILog logger)
        {
            _logger = logger;
        }

        public void AddLogLines(string logMessage, int rowCount)
        {
            
            for (int i = 0; i < rowCount; i++)
            {
                Console.WriteLine("Before loop");
                _logger.Write(logMessage + i.ToString());
            }
            
        }

        public void StopLoggingWithFlush(bool withFlush)
        {
            if (withFlush)
            {
                _logger.StopWithFlush();
            }
            else
            {
                _logger.StopWithoutFlush();
            }

        }
    }
}
