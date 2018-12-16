using System;

namespace CodeCraft.Logger.ProducerConsumer
{
    public interface IProducerConsumer<T> : IDisposable
    {
        void Produce(T datas);
    }

}
