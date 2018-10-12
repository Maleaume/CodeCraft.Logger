using System;

namespace CodeCraft.Logger.ProducerConsumer
{
    public interface ILogProducerConsumer : IDisposable
    {
        void Enqueue(string log);
    }

}
