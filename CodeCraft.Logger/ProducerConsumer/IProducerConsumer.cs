using System;
using System.Threading.Tasks;

namespace CodeCraft.Logger.ProducerConsumer
{
    /// <summary>
    /// Producer/Consumer interface
    /// </summary>
    /// <typeparam name="T">The type of data that this class operates on.</typeparam>
    public interface IProducerConsumer<in T> : IBaseProducerConsumer
    {
        void Produce(T item);
    }

    public interface IBaseProducerConsumer
    {
        Task Start();
        Task Stop();
        bool IsStopped { get; }
        int InputCount { get; }
        event EventHandler<EventArgs> ConsumeEnded;
    }

}
