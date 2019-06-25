using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.UnitTestNet
{
    [TestClass]
    public class UnitTest1
    {
        ConsoleLogger ConsoleLogger = new ConsoleLogger();
        [TestMethod]
        public void ConsoleLoggerUsingAttributes()
        {
            var t1 = new Thread(new ParameterizedThreadStart(TraceEvery300msLogs));
            t1.Start(ConsoleLogger);
            t1.Join();

            //  ConsoleLogger.Dispose();

        }

        [TestMethod]
        public void ConsoleLoggerInFunction()
        {
            var logger = new ConsoleLogger();
            {
                var t1 = new Thread(new ParameterizedThreadStart(TraceEvery300msLogs));
                t1.Start(ConsoleLogger);
                t1.Join();
            } ;

        }
 

        private void TraceEvery300msLogs(object logger) => TraceEvery300msLogs(((ILogger)logger).Trace);
        private void TraceEvery300msLogs(ILogger logger) => TraceEvery300msLogs(logger.Trace);
        private void TraceEvery300msLogs(LogLevel log)
        {
            for (int i = 0; i < 3; i++)
            {
                log(i.ToString());
                Thread.Sleep(300);
            }
        }

        [TestMethod]
        public void ConsoleLoggerWithDisposeCancellation()
        {
            var t1 = new Thread(new ThreadStart(TraceLogs));
            var t2 = new Thread(new ThreadStart(InfoLogs));
            var t3 = new Thread(new ThreadStart(DebugLogs));

            t2.Start();
            t1.Start();
            t3.Start();
            t3.Join();
            t1.Join();
            t2.Join();
            
        }

        [TestMethod]
        public void ConsoleLoggerWithoutDisposeCancellation()
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
            Debug.WriteLine("BeforeSleep");
            int i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;
            while (i < int.MaxValue) { i++; }
             i = 0;


            Debug.WriteLine("EndSleep");
            var t11 = new Thread(new ThreadStart(TraceLogs));
            var t21 = new Thread(new ThreadStart(InfoLogs));
            var t31 = new Thread(new ThreadStart(DebugLogs));

            t11.Start();
            t21.Start();
            t31.Start();


            t11.Join();
            t31.Join();
            t21.Join();
            Debug.WriteLine("BeforeSleep");
            Thread.Sleep(10000);
            Debug.WriteLine("EndSleep");
        }


        delegate void LogLevel(string log);

        private void TraceLogs() => Logs(ConsoleLogger.Trace);
        private void InfoLogs() => Logs2(ConsoleLogger.Info);
        private void DebugLogs() => Logs3(ConsoleLogger.Debug);

        private void Logs(LogLevel Log)
        {
            Thread.Sleep(1000);
            for (int i = 0; i < 200; i++)
            {
                Thread.Sleep(50);
                Log(i.ToString());
            }
            //
            for (int i = 1000; i < 1200; i++)
                Log(i.ToString());
            Log("1#################################");
        }
        private void Logs2(LogLevel Log)
        {
            for (int i = 0; i < 200; i++)
            {
                Log(i.ToString()); Thread.Sleep(5);
            }
            Log("2#################################");
        }

        private void Logs3(LogLevel Log)
        {
            for (int i = 0; i < 200; i++)
            {
                Log(i.ToString());
                Thread.Sleep(2);
            }
            Log("3#################################");
        }
    }
}
