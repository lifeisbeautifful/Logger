namespace LogComponent.Abstract
{
    /// <summary>
    /// Created ILoggerCreator interface to implement Factory method pattern for creating loggers.
    /// </summary>
    public interface ILoggerCreator
    {
        /// <summary>
        /// Returns instance of ILog.
        /// </summary>
        /// <returns></returns>
        ILog CreateLogger();
    }
}
