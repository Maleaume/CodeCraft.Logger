namespace CodeCraft.Logger.ProducerConsumer
{
    public abstract class LogProducerConsumer : ProducerConsumer<string>, ILogProducerConsumer
    {
        protected LogProducerConsumer()
            :base("LoggerThread")
        {
        }
         
        protected abstract void WriteLog(string log);

        protected override void Process(string log) => WriteLog(log); 
    }
}
