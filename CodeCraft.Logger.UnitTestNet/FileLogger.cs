
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;

namespace CodeCraft.Logger.UnitTestNet
{
    [TestClass]
    public class FileLogger
    {
        Logger.FileLogger loggerA = new Logger.FileLogger(@"E:\Log.txt");

        Logger.FileLogger loggerB = new Logger.FileLogger(@"E:\Log.txt");
        [TestInitialize]
        public void Initialize()
        {
            var logoutput = @"D:\Log.txt";
            if (File.Exists(logoutput))
                File.Delete(logoutput);
        }

        public void Cleanup()
        {
            try
            {
                var logoutput = @"E:\Log.txt";
                Assert.IsTrue(File.Exists(logoutput));
                Assert.AreEqual(81, File.ReadAllLines(logoutput).Length);
            }
            catch (IOException ex)
            {
                Thread.Sleep(5500);

            }


        }

        [TestMethod]
        public void FileLoggerTest()
        {
            /*using (*/
            var fileLogger = new Logger.FileLogger(@"E:\Log.txt");

            fileLogger.Error("Tests");
            for (int i = 0; i < 90; i++)
            {
                fileLogger.Warn($"{i}");
            }
            Thread.Sleep(2500);

            for (int i = 0; i < 20; i++)
            {
                fileLogger.Warn($"{i}");
            }
            Thread.Sleep(2500);
        }


        [TestMethod]
        public void SameFileLoggerTest()
        {
            /*using (*/
         

            var t1 = new Thread(new ThreadStart(TraceLogs));
            var t2 = new Thread(new ThreadStart(InfoLogs));
         

            t2.Start(  );
            t1.Start( );  
            t1.Join();
            t2.Join();
  
        }
        private void TraceLogs() => Logs(loggerA.Trace);
        private void InfoLogs() => Logs(loggerB.Info);

        delegate void LogLevel(string log);
        private void Logs (LogLevel Log)
        {
            for (int i = 0; i < 20; i++)
            {
                Log(i.ToString()); Thread.Sleep(5);
            } 
        }
    }
}
