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
        protected bool disposed = false;
        /// <summary>
        /// Task for consumer.
        /// </summary>
        protected readonly Task ConsumerTask;
        /// <summary>
        /// Data storage.
        /// </summary>
        protected readonly BlockingCollection<T> DataQueue = new BlockingCollection<T>();

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
        private Task InitializeConsumerTask() => new Task(() => Consume(), cancelToken, TaskCreationOptions.HideScheduler);

        /// <summary>
        /// Start consumer task.
        /// </summary>
        public void StartConsumerTask() => ConsumerTask.Start();
        public void WaitConsumer() => ConsumerTask.Wait();

        public void CompleteAdding() => DataQueue.CompleteAdding();
        /// <summary>
        /// Produce (Add) new data.
        /// </summary>
        /// <param name="data">Data to add</param>
        public void Produce(T data)
        {
            DataQueue.Add(data);
       
        }
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
               // while (!DataQueue.IsCompleted)
                {
                    // Blocks if number.Count == 0
                    // IOE means that Take() was called on a completed collection.
                    // Some other thread can call CompleteAdding after we pass the
                    // IsCompleted check but before we call Take. 
                    // In this example, we can simply catch the exception since the 
                    // loop will break on the next iteration. 
                  
                    ConsumeData();
                   
                    //if (tokenSource.IsCancellationRequested)
                    {
                        // Debug.WriteLine("Cancel");
                        // ConsumeEnumarable();
                        // break;
                    }

                }

            }
            catch (ProducerConsumerException ex)
            {
                ConsumeEnumerable();
            }


            void ConsumeData()
            {
                 foreach (var l in DataQueue.GetConsumingEnumerable()) Process(l);

            }


            void ConsumeEnumerable()
            {
                while (DataQueue.TryTake(out var data))
                {
                    Process(data);
                }
            }
        }

        /// <summary>
        ///  Releases resources used by the CodeCraft.Logger.ProducerConsumer
        //     instance.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;

            // ConsumerTask.Wait();
            // DataQueue.CompleteAdding();
            // tokenSource.Cancel();

            //
           


            // tokenSource.Cancel();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            WaitConsumerEnded();

            if (disposing)
                DataQueue.Dispose();
            disposed = true;
        }
        protected virtual void  WaitConsumerEnded()
        {
            DataQueue.CompleteAdding();
            while (!(ConsumerTask.IsCompleted || ConsumerTask.IsCanceled)) ;
        }


        /// <summary>
        /// Destructor to substitute Object.Finalize.
        /// </summary>
        ~ProducerConsumer() => Dispose(false);
    }

}
