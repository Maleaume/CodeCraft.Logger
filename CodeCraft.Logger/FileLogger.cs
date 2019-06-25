using CodeCraft.Logger.ProducerConsumer;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
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
