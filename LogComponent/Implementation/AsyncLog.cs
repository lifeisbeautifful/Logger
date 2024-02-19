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

        private DateTime _curDate = DateTime.Now;

        public AsyncLog()
        {
            if (!Directory.Exists(@"C:\LogTest"))
                Directory.CreateDirectory(@"C:\LogTest");

            _writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            _writer.AutoFlush = true;
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
                                {
                                    continue;
                                }

                                if (!_exit || _QuitWithFlush)
                                {

                                    _handled.Add(logLine);

                                    StringBuilder stringBuilder = new StringBuilder();

                                    if ((DateTime.Now - _curDate).Days != 0)
                                    {
                                        _curDate = DateTime.Now;

                                        _writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

                                        _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

                                        stringBuilder.Append(Environment.NewLine);

                                        _writer.Write(stringBuilder.ToString());

                                        _writer.AutoFlush = true;
                                    }

                                    stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                                    stringBuilder.Append("\t");
                                    stringBuilder.Append(logLine.LineText());
                                    stringBuilder.Append("\t");

                                    stringBuilder.Append(Environment.NewLine);

                                    _writer.Write(stringBuilder.ToString());
                                }
                            }
                        }


                        for (int y = 0; y < _handled.Count; y++)
                        {
                            //Limit access, so the list of lines can be accessible only for 
                            //one thread at once.
                            lock (_lines)
                            {
                                _lines.Remove(_handled[y]);
                            }
                        }

                        //Limit access, so the list of lines can be accessible only for 
                        //one thread at once.
                        lock (_lines)
                        {
                            if (_QuitWithFlush == true && _lines.Count == 0)
                            {
                                _exit = true;
                            }

                        }

                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex) { }
            }
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

        private void AddLogLine(string text)
        {
            //Limit access, so the list of lines can be accessible only for 
            //one thread at once.
            lock (_lines)
            {
                _lines.Add(new LogLine() { Text = text, Timestamp = DateTime.Now });
            }
        }

        public void AddLinesDecreaseCount(string logMessage, int rowCount)
        {
            for (int i = rowCount; i > 0; i--)
            {
                AddLogLine(logMessage + i.ToString());
            }
        }

        public void AddLinesIncreaseCount(string logMessage, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                AddLogLine(logMessage + i.ToString());
            }
        }
    }
}