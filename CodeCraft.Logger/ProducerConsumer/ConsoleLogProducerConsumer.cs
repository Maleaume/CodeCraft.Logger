namespace CodeCraft.Logger.ProducerConsumer
{
    public sealed class ConsoleLogProducerConsumer : LogProducerConsumer
    {
        protected override void WriteLog(string log) => System.Diagnostics.Debug.WriteLine(log);
    }

}
