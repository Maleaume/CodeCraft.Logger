using CodeCraft.Logger.ProducerConsumer;

namespace CodeCraft.Logger
{
    public class FileLogger : BaseLogger<FileLogProducerConsumer>
    {
        public string FilePath
        {
            get => logProducerConsumer.FileLogPath;
            private set => logProducerConsumer.FileLogPath = value;
        }

        public FileLogger(string filePath)
        {
            FilePath = filePath; 
        } 
        
    }
}
