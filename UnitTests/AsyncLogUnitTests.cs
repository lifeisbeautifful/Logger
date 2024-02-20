using LogComponent.Abstract;
using LogComponent.Implementation;
using NUnit.Framework;


namespace UnitTests
{
    [TestFixture]
    public class AsyncLogUnitTests
    {
        private string folderPath = @"C:\LogTest";
        private DateProvider _dateProvider = new DateProvider();

        
        [SetUp]
        public void Setup()
        {

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

        }

        [Test]
        public void AsyncLogInstanceCreatesDir()
        {
            ILog logger = new AsyncLog(_dateProvider);
            logger.CloseStreamWritter();

            Assert.That(Directory.Exists(folderPath), "Directory was not created"); 
        }

        [Test]
        public void AsyncLogInstanceCreatesFile()
        {
            ILog logger = new AsyncLog(_dateProvider);
            logger.CloseStreamWritter();

            var logFile = Directory.GetFiles(folderPath);
            var file = File.ReadAllTextAsync(logFile.First());
            

            Assert.Multiple(() =>
            {
                Assert.That(logFile.Count(), Is.EqualTo(1), "log files quantity are not as expected after create a looger");
                Assert.That(File.Exists(logFile.First()), "Expected file was not created");
                Assert.That(file.Result.Contains("Timestamp"), "Timestamp column is not present in file context");
                Assert.That(file.Result.Contains("Data"), "Data column is not present in file context");
            });
        }


        [TestCase("Test add lines with flush: ", 150)]
        public void StopWithFlush(string text, int rows)
        {
            ILog logger = new AsyncLog(_dateProvider);
            logger.StartLogging();

            logger.AddLinesIncreaseCount("Test add lines with flush: ", 150);
            logger.StopWithFlush();
            Thread.Sleep(2000);

            var fileContent = File.ReadAllTextAsync(Directory.GetFiles(folderPath).First());
            var logLinesArr = fileContent.Result.Split("\n");

            ////Added 2 rows to rows quantity are: last empty string + header
            Assert.AreEqual(rows+2, logLinesArr.Length, "Created log lines quantity is not as expected");
            Assert.That(fileContent.Result.Contains(text + (rows - 1).ToString()), "Log file does not contain expected context");
        }

        [TestCase("Test add lines with flush: ", 150)]
        public void StopWithoutFlush(string text, int rowCount)
        {
            ILog logger = new AsyncLog(_dateProvider);
            logger.StartLogging();

            logger.AddLinesIncreaseCount("Test add lines with flush: ", 150);
            Thread.Sleep(1000);
            logger.StopWithoutFlush();
            Thread.Sleep(2000);
            
            var fileContent = File.ReadAllText(Directory.GetFiles(folderPath).First());
            var logLinesArr = fileContent.Split("\n");
            int expectedLoggedRows = 50;

            Assert.That(logLinesArr.Length, Is.LessThan(rowCount), "Log lines quantity is not less as expected");
            Assert.That(logLinesArr.Length, Is.AtLeast(expectedLoggedRows), "Log lines quantity is not at least as expected");
        }

        [TestCase(2024, 02, 19)]
        public void TestNewFileCreatingAfterMidnight(int year, int month, int day)
        {
            var priorDate = new DateProvider();
            priorDate.Now = new DateTime(year, month, day);

            ILog logger = new AsyncLog(priorDate);
            logger.StartLogging();

            logger.AddLinesIncreaseCount("Test new folder creation after midnight");
            Thread.Sleep(2000);
            var logFiles = Directory.GetFiles(folderPath);

            Assert.That(logFiles.Count, Is.EqualTo(2), $"{logFiles.Count()} log files were created instead of 2");
        }
    }
}