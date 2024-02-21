using System.Text;
using LogComponent.Abstract;


namespace LogComponent.Implementation
{
    public class AsyncLog : ILog
    {
        private Thread _runThread;

        private List<LogLine> _lines = new List<LogLine>();

        private StreamWriter _writer;

        private bool _exit;

        private bool _QuitWithFlush = false;

        private DateTime _curDate;

        private DateProvider _dateProvider;

        private readonly string dirPath = @"C:\LogTest";


        public AsyncLog(DateProvider provider)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            _dateProvider = provider;

            _curDate = _dateProvider.Now;

            CreateLogFile();

            //_writer = File.AppendText(dirPath + @"\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

            //_writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            //_writer.AutoFlush = true;
        }

       
        private void Log()
        {
            while (!_exit)
            {
                //Catch exception in log loop, so application continues to run if an error occur.
                try
                {
                    if (_lines.Count > 0)
                    {
                        int f = 0;
                        List<LogLine> _handled = new List<LogLine>();

                        //Limit access, so the list of lines can be accessible only for 
                        //one thread at once.
                        lock (_lines)
                        {
                            foreach (LogLine logLine in _lines)
                            {
                                f++;

                                if (f > 5)
                                    continue;
                                

                                if (!_exit || _QuitWithFlush)
                                {
 
                                    _handled.Add(logLine);

                                     StringBuilder stringBuilder = new StringBuilder();

                                     if ((DateTime.Now - _curDate).Days != 0)
                                     {
                                        _curDate = DateTime.Now;
                                        CreateLogFile();
                                     }

                                    WriteDataToFile(logLine, stringBuilder);
                                }
                            }
                        }

                        DeleteLoggedDataFromLogList(_handled);
                        CheckIfLoggingIsFinished();

                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex) { }
            }

            CloseLogWritter();
        }


 
        public void CloseLogWritter()
        {
            this._writer.Dispose();
            GC.SuppressFinalize(this);
        }


        public void StartLogging()
        {
            _runThread = new Thread(Log);
            _runThread.Start();
        }


        public void StopWithoutFlush()
        {
            _exit = true;
        }


        public void StopWithFlush()
        {
            _QuitWithFlush = true;
        }


        public void AddLinesDecreaseCount(string logMessage, int rowCount = 50)
        {
            for (int i = rowCount; i > 0; i--)
            {
                AddLogLine(logMessage + i.ToString());
            }
        }


        public void AddLinesIncreaseCount(string logMessage, int rowCount = 14)
        {
            for (int i = 0; i < rowCount; i++)
            {
                AddLogLine(logMessage + i.ToString());
            }
        }


        /// <summary>
        /// Set exit criteria for logging loop to true if logline list is empty
        /// and StopWithFlush method was called.
        /// </summary>
        private void CheckIfLoggingIsFinished()
        {
            //Limit access, so the list of lines can be accessible only for 
            //one thread at once.
            lock (_lines)
            {
                if (_QuitWithFlush == true && _lines.Count == 0)
                {
                    _exit = true;
                }
            }
        }


        /// <summary>
        /// Delete already written to log file data from logline list.
        /// </summary>
        /// <param name="loggedLines">List with already logged lines.</param>
        private void DeleteLoggedDataFromLogList(List<LogLine> loggedLines)
        {
            for (int y = 0; y < loggedLines.Count; y++)
            {
                //Limit access, so the list of lines can be accessible only for 
                //one thread at once.
                lock (_lines)
                {
                    _lines.Remove(loggedLines[y]);
                }
            }
        }


        /// <summary>
        /// Writes log line with timestamp to file.
        /// </summary>
        /// <param name="logLine">List of lines with not logged yet data.</param>
        /// <param name="stringBuilder">object to build appropriate(with all needed info) line for logging.</param>
        private void WriteDataToFile(LogLine logLine, StringBuilder stringBuilder)
        {
            stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            stringBuilder.Append("\t");
            stringBuilder.Append(logLine.LineText());
            stringBuilder.Append("\t");

            stringBuilder.Append(Environment.NewLine);

            _writer.Write
            (stringBuilder.ToString());
        }


        /// <summary>
        /// Creates new file with predefined name and header.
        /// </summary>
        private void CreateLogFile()
        {
            _writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            _writer.AutoFlush = true;
        }


        /// <summary>
        /// Creates new logline with text from param and current date & time.
        /// Addes those line to logLine list.
        /// </summary>
        /// <param name="text">Text which should be included into log line.</param>
        private void AddLogLine(string text)
        {
            //Limit access, so the list of lines can be accessible only for 
            //one thread at once.
            lock (_lines)
            {
                _lines.Add(new LogLine() { Text = text, Timestamp = DateTime.Now });
            }
        }
    }
}