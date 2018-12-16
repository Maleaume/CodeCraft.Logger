using System;

namespace CodeCraft.Logger.ProducerConsumer
{
    public interface IProducerConsumer<T> : IDisposable
    {
        void Enqueue(T datas);
    }

}
