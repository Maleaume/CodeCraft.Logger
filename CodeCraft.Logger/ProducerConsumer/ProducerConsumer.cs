using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CodeCraft.Logger.ProducerConsumer
{
    /// <summary>
    /// Abstract class to implement a simple producer/consumer pattern.
    /// </summary>
    /// <typeparam name="T">Type of data to produce and consume.</typeparam>
    public abstract class ProducerConsumer<T> : IProducerConsumer<T>
    {
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
            ConsumerTask = InitializeConsumerTask();
        }

        /// <summary>
        /// Initialize consumer Task.
        /// </summary>
        /// <returns>New task that contains consumer processing.</returns>
        private Task InitializeConsumerTask() => new Task(() => Consume());

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

        /// <summary>
        /// Consumer processing.
        /// </summary>
        private void Consume()
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
                    if (DataQueue.TryTake(out T data))
                        Process(data);
                }
                catch (InvalidOperationException ex)
                {
                }
            }

        }

        /// <summary>
        ///  Releases resources used by the CodeCraft.Logger.ProducerConsumer
        //     instance.
        /// </summary>
        public virtual void Dispose()
        {
            DataQueue.CompleteAdding();
            while (!DataQueue.IsCompleted) ;
            DataQueue.Dispose();
        }

    }
}
