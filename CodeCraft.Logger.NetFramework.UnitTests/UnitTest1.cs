using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CodeCraft.Logger.ProducerConsumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.NetFramework.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        ConsoleLogger ConsoleLogger = new ConsoleLogger();
        [TestMethod]
        public void ConsoleLoggerTest2()
        {

            var t1 = new Task(() =>TraceEvery300msLogs());
          
            t1.Start();
             
            t1.Wait();
          
        //  ConsoleLogger.Dispose();

        }
        private void TraceEvery300msLogs() => TraceEvery300msLogs(ConsoleLogger.Trace);
        private void TraceEvery300msLogs(LogLevel log)
        {
            for (int i = 0; i <3 ; i++)
            {
                log(i.ToString());
                Thread.Sleep(300);
            }
        }




        [TestMethod]
        public void ConsoleLoggerTest()
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

         // ConsoleLogger.Dispose();
        }
        delegate void LogLevel(string log);

        private void TraceLogs() => Logs(ConsoleLogger.Trace);
        private void InfoLogs() => Logs2(ConsoleLogger.Info);
        private void DebugLogs() => Logs3(ConsoleLogger.Debug);

        private void Logs(LogLevel Log)
        {Thread.Sleep(1000);
            for (int i = 0; i < 1000; i++)
                Log(i.ToString());
            //
            for (int i = 100; i < 120; i++)
                Log(i.ToString());
            Debug.WriteLine("#################################");
        }
        private void Logs2(LogLevel Log)
        {
            for (int i = 0; i < 1000; i++)
            { Log(i.ToString());//Thread.Sleep(5);
            }
          Debug.WriteLine("#################################");
        }

        private void Logs3(LogLevel Log)
        {
            for (int i = 0; i < 1000; i++)
            { Log(i.ToString());
                //Thread.Sleep(2);
            }
            Debug.WriteLine("#################################");
        }
        [TestMethod]    
        public void FileLoggerTest()
        {
            using (var fileLogger = new FileLogger(@"D:\Log.txt"))
            {
                fileLogger.Error("Tests");
                for (int i = 0; i < 2200; i++)
                    fileLogger.Warn($"{i}");    
            }
        }
    }
}
