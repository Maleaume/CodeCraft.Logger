using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CodeCraft.Logger.ProducerConsumer
{
    /// <summary>
    /// Abstract class to implement a simple producer/consumer pattern.
    /// </summary>
    /// <typeparam name="T">Type of data to produce and consume.</typeparam>
    public abstract class ProducerConsumer<T> : IProducerConsumer<T>
    {
        /// <summary>
        /// Concurrent queue where item to process are stored.
        /// </summary>
        private readonly ActionBlock<T> queue;
        protected virtual void OnProcessEnded(EventArgs e)
        {
            ConsumeEnded?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> ConsumeEnded;
        /// <summary>
        /// Retrieve the queue count.
        /// </summary>
        public int InputCount => queue.InputCount;
        /// <summary>
        /// Method to umplement in child classes to consume current item
        /// </summary>
        /// <param name="item">Item to consume</param>
        protected abstract void Consume(T item);

        /// <summary>
        /// Create a new instance of Producer/Consumer
        /// </summary>
        /// <param name="maxDegreeOfParallelism">Max degree Of parallelism</param>
        protected ProducerConsumer(int maxDegreeOfParallelism)
        {
            var consumerOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            queue = new ActionBlock<T>(item => Consume(item), consumerOptions);
        }

        /// <summary>
        /// This method adds an element to the processing queue 
        /// </summary>
        /// <param name="item">item to add.</param>
        public void Produce(T item)
        { 
            queue.Post(item);
        }

        /// <summary>
        /// Start consumer task
        /// </summary>
        /// <returns>awaitable Task</returns>
        public Task Start()
        {
            IsStopped = false;
            return queue.Completion;
        }

        /// <summary>
        /// Stop consumer task
        /// </summary>
        /// <returns>awaitable task</returns>
        public virtual  void Stop()
        {
            queue.Complete();
             
            IsStopped = true;
        }
        public bool IsStopped { get; private set; }
        public bool StopConditionsReached() => false;

        ~ProducerConsumer()
        {
            Stop();
        }
    }
}
