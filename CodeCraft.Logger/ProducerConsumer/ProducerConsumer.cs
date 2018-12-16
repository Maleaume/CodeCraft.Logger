using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCraft.Logger.ProducerConsumer
{
    public abstract class ProducerConsumer<T> : IProducerConsumer<T>
    {
        readonly Task LoggingTask;
        private readonly BlockingCollection<T> DataQueue = new BlockingCollection<T>();

        protected ProducerConsumer(string threadName)
        { 
            LoggingTask = new Task(() => ProcessQueue()); 
        }

        protected void StartProcessThread() => LoggingTask.Start();

        public void Enqueue(T data)
        {
            DataQueue.Add(data); 
        }

        protected abstract void Process(T Data);

        private void ProcessQueue()
        {
            while (!DataQueue.IsCompleted)
            {
              
                // Blocks if number.Count == 0
                // IOE means that Take() was called on a completed collection.
                // Some other thread can call CompleteAdding after we pass the
                // IsCompleted check but before we call Take. 
                // In this example, we can simply catch the exception since the 
                // loop will break on the next iteration.
                try
                {
                   if (DataQueue.TryTake( out T data))
                    Process(data);
                   // Process(DataQueue.Take());
                }
                catch (InvalidOperationException ex) {
                }
                catch (OperationCanceledException ) { }

                
            }
 
        }

        public virtual void Dispose()
        {
            DataQueue.CompleteAdding();
            while (!DataQueue.IsCompleted) ;
            DataQueue.Dispose(); 
        }

    }
}
