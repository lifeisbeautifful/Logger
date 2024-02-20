namespace LogComponent.Abstract
{
    public interface ILog
    {
        /// <summary>
        /// Creates separate thread and start logging loop.
        /// </summary>
        void StartLogging();


        /// <summary>
        /// Calls dispose method to close the stream.
        /// </summary>
        void CloseStreamWritter();


        /// <summary>
        /// Stop the logging. If any outstadning logs theses will not be written to Log.
        /// </summary>
        void StopWithoutFlush();


        /// <summary>
        /// Stop the logging. The call will not return until all logs have been written to Log.
        /// </summary>
        void StopWithFlush();


        /// <summary>
        /// Add numbered(started from rowCount and decrese to 1) list lines to Log.
        /// The method will satisfy demands of writing logs and could be adjusted if line 
        /// quantity should be changed.
        /// </summary>
        /// <param name="logMessage">The text to written to the log.</param>
        /// <param name="rowCount">Max quantity of text lines which could be added to Log.</param>
        void AddLinesDecreaseCount(string logMessage, int rowCount = 50);


        /// <summary>
        /// Add numbered(started from 0 and increase up to rowCount) list lines to Log.
        /// The method will satisfy demands of writing logs and could be adjusted if line 
        /// quantity should be changed.
        /// </summary>
        /// <param name="logMessage">The text to written to the log.</param>
        /// <param name="rowCount">Max quantity of text lines which could be added to Log.</param>
        void AddLinesIncreaseCount(string logMessage, int rowCount = 14);

    }
}
