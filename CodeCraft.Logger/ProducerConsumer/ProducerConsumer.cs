using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCraft.Logger.ProducerConsumer
{
    /// <summary>
    /// Abstract class to implement a simple producer/consumer pattern.
    /// </summary>
    /// <typeparam name="T">Type of data to produce and consume.</typeparam>
    public abstract class ProducerConsumer<T> : IProducerConsumer<T>
    {
        private bool disposed = false;
        /// <summary>
        /// Task for consumer.
        /// </summary>
        private readonly Task ConsumerTask;
        /// <summary>
        /// Data storage.
        /// </summary>
        private readonly BlockingCollection<T> DataQueue = new BlockingCollection<T>();

        /// <summary>
        ///  Initializes a new instance of the CodeCraft.Logger.ProducerConsumer
        /// </summary>
        /// <param name="threadName"></param>
        protected ProducerConsumer(string threadName)
        {
            cancelToken = tokenSource.Token;
            ConsumerTask = InitializeConsumerTask();
        }

        /// <summary>
        /// Initialize consumer Task.
        /// </summary>
        /// <returns>New task that contains consumer processing.</returns>
        private Task InitializeConsumerTask() => new Task(() => Consume(), cancelToken, TaskCreationOptions.RunContinuationsAsynchronously);

        /// <summary>
        /// Start consumer task.
        /// </summary>
        protected void StartConsumerTask() => ConsumerTask.Start();

        /// <summary>
        /// Produce (Add) new data.
        /// </summary>
        /// <param name="data">Data to add</param>
        public void Produce(T data) => DataQueue.Add(data);

        /// <summary>
        /// <see langword="abstract"/> method that contains process to apply on data.
        /// </summary>
        /// <param name="data">Data to process when consumer have it.</param>
        protected abstract void Process(T data);

        protected CancellationTokenSource tokenSource = new CancellationTokenSource();
        protected CancellationToken cancelToken;
        /// <summary>
        /// Consumer processing.
        /// </summary>
        private void Consume()
        {
            try
            {
                while (!DataQueue.IsCompleted)
                {
                    // Blocks if number.Count == 0
                    // IOE means that Take() was called on a completed collection.
                    // Some other thread can call CompleteAdding after we pass the
                    // IsCompleted check but before we call Take. 
                    // In this example, we can simply catch the exception since the 
                    // loop will break on the next iteration. 
                    ConsumeData();
                    if (tokenSource.IsCancellationRequested)
                    {
                        ConsumeEnumarable();
                        break;
                    }

                }

            }
            catch (ProducerConsumerException)
            {
                ConsumeEnumarable();
            }

  
            void ConsumeData()
            {
                var data = default(T);
                try
                {
                    data = DataQueue.Take(cancelToken);
                    Process(data);
                }
                catch (Exception ex) { 
                    if (ex is ThreadAbortException || ex is OperationCanceledException)
                    {
                        Process(Data);
                        throw new ProducerConsumerException("Operation was canceled", ex);
                    }
                    throw;
                }
            }


            void ConsumeEnumarable()
            {
                while (DataQueue.TryTake(out var data))
                    Process(data);
            }
        }

        /// <summary>
        ///  Releases resources used by the CodeCraft.Logger.ProducerConsumer
        //     instance.
        /// </summary>
        public virtual void Dispose()
        {
            DataQueue.CompleteAdding();
            tokenSource.Cancel();

            while (!(ConsumerTask.IsCompleted || ConsumerTask.IsCanceled)) ;
            // tokenSource.Cancel();
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DataQueue.Dispose();
            }
            disposed = true;
        }

        /// <summary>
        /// Destructor to substitute Object.Finalize.
        /// </summary>
        ~ProducerConsumer() => Dispose(false);
    }
}
