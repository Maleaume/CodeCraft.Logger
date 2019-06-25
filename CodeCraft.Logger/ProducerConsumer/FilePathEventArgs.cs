using System;

namespace CodeCraft.Logger.ProducerConsumer
{
    public class FilePathEventArgs : EventArgs
    {
        public string FilePath { get; }

        public FilePathEventArgs(string filepath)
        {
            FilePath = filepath;
        }
    }
}
