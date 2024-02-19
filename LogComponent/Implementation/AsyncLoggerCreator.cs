using LogComponent.Abstract;

namespace LogComponent.Implementation
{
    /// <summary>
    /// Added AsyncLoggerCreator class which implements ILoggerCreator method
    /// for creating concrete instance(AsyncLog in this case).
    /// </summary>
    public class AsyncLoggerCreator : ILoggerCreator
    {
        public ILog CreateLogger()
        {
            return new AsyncLog();
        }
    }
}
