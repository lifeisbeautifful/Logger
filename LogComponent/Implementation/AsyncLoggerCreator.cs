using LogComponent.Abstract;

namespace LogComponent.Implementation
{
    /// <summary>
    /// Added AsyncLoggerCreator class which implements ILoggerCreator method
    /// for creating concrete instance(AsyncLog in this case).
    /// </summary>
    public class AsyncLoggerCreator : ILoggerCreator
    {
        private readonly ILog _logger;
        private readonly DateProvider _dateProvider;

        public AsyncLoggerCreator(ILog logger, DateProvider provider)
        {
            _logger = logger;
            _dateProvider = provider;
        }

        public ILog CreateLogger()
        {
            return _logger;
        }
    }
}
