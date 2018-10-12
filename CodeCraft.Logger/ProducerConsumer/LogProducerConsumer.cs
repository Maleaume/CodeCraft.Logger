using System.Collections.Concurrent;
using System.Threading;

namespace CodeCraft.Logger.ProducerConsumer
{
    public  abstract class LogProducerConsumer : ILogProducerConsumer
    {
        readonly WaitHandle[] WaitHandleEvent;
        readonly Thread LoggingThread;

        protected ManualResetEvent hasNewItems = new ManualResetEvent(false);
        protected ManualResetEvent terminate = new ManualResetEvent(false);
        protected ManualResetEvent waiting = new ManualResetEvent(false);

        private ConcurrentQueue<string> LogsQueue = new ConcurrentQueue<string>();

        public LogProducerConsumer()
        {
            WaitHandleEvent = new WaitHandle[] { hasNewItems, terminate };
            LoggingThread = new Thread(new ThreadStart(ProcessQueue))
            {
                IsBackground = true,
                Name = "LoggerThread"
            };
            LoggingThread.Start();
        }
        protected abstract void WriteLog(string log);

        private void ProcessQueue()
        {
            int i = -1;
            while (true)
            {
                waiting.Set();
                i = WaitHandle.WaitAny(WaitHandleEvent, -1);
                hasNewItems.Reset();
                waiting.Reset();
                DeQueue();
                if (i == 1)
                    return;
            }

            void DeQueue()
            {
                while (LogsQueue.TryDequeue(out string log))
                    WriteLog(log);
            }
        }

        public void Enqueue(string log)
        {
            LogsQueue.Enqueue(log);
            hasNewItems.Set();
        }

        public void Dispose()
        {
            terminate.Set();
            while (LoggingThread.IsAlive) ;
        }
    }

}
