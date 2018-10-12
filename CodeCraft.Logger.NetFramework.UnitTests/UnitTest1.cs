using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.NetFramework.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        ConsoleLogger ConsoleLogger = new ConsoleLogger();
        [TestMethod]
        public void TestMethod1()
        {
            var t1 = new Thread(new ThreadStart(TraceLogs));
            var t2 = new Thread(new ThreadStart(InfoLogs));
            var t3 = new Thread(new ThreadStart(DebugLogs));

            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t3.Join();
            t2.Join();

            ConsoleLogger.Dispose();
        }
        delegate void LogLevel(string log);

        private void TraceLogs() => Logs(ConsoleLogger.Trace);
        private void InfoLogs() => Logs(ConsoleLogger.Info);
        private void DebugLogs() => Logs(ConsoleLogger.Debug);

        private void Logs(LogLevel Log)
        {
            for (int i = 0; i < 800; i++)
                Log(i.ToString());
            Thread.Sleep(1000);
            for (int i = 100; i < 120; i++)
                Log(i.ToString());
        }



    }
}
