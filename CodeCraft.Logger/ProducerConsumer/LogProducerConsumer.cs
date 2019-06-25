namespace CodeCraft.Logger.ProducerConsumer
{
    public abstract class LogProducerConsumer : ProducerConsumer<string>, ILogProducerConsumer
    {
        protected LogProducerConsumer()
            : base(1)
        {
        }

        protected abstract void WriteLog(string log);

        protected override void Consume(string log) => WriteLog(log);

    }
}
