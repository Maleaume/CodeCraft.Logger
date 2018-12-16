using System; 
using System.IO; 

namespace CodeCraft.Logger.ProducerConsumer
{
    public sealed class ConsoleLogProducerConsumer : LogProducerConsumer
    {
        public ConsoleLogProducerConsumer() => StartConsumerTask();

        protected override void WriteLog(string log) {  Console.WriteLine(log); }
    }

    public delegate void FilePathCompletedEventHandler(object sender, FilePathEventArgs e);

    public class FilePathEventArgs : EventArgs
    {
        public string FilePath { get; }

        public FilePathEventArgs(string filepath)
        {
            FilePath = filepath;
        }
    }

    public class FileManager
    {
        public static StreamWriter CreateOrOpenFile(string filePath)
        {
            return new StreamWriter(filePath, true);
        }

        public static async void WriteTextAsync(string filePath, string text)
        {
            using (var outputFile = new StreamWriter(filePath, true))
            {
                await outputFile.WriteAsync(text);
            }
        }

        public static  void WriteText (StreamWriter stream, string text)
        {
             stream.Write(text);

        }
    }


    public sealed class FileLogProducerConsumer : LogProducerConsumer
    {
        /// <summary>
        /// Result of asynchrone call when File path is set.
        /// Need to keep it in case of dispose is called before end of async task.
        /// </summary>
        private IAsyncResult InitializeAsyncResult;
        private StreamWriter StreamWriter;
        private event FilePathCompletedEventHandler FilePathEvent;
        private string fileLogPath;
        private bool InitializeMode = false;
        public string FileLogPath
        {
            get { return fileLogPath; }
            set
            {
                fileLogPath = value;
                SendFilePathEvent();

            }
        }
        public FileLogProducerConsumer()
        {
            FilePathEvent += FileLogProducerConsumer_FilePathEvent;
        }

        private void FileLogProducerConsumer_FilePathEvent(object sender, FilePathEventArgs e)
        {
            try
            { 
                StreamWriter = FileManager.CreateOrOpenFile(fileLogPath);
                
            }
            catch (Exception ex)
            {
            }
        }

        private void SendFilePathEvent()
        {
            InitializeMode = true;
            var args = new FilePathEventArgs(fileLogPath);
           InitializeAsyncResult = FilePathEvent.BeginInvoke(this, args, (result) => FilePathSettingsEnd(), null);
         
        }
        void FilePathSettingsEnd()
        {
            StartConsumerTask();
            InitializeMode = false;
        }
        public override void Dispose()
        {
            InitializeAsyncResult.AsyncWaitHandle.WaitOne();
            while (InitializeMode) ;
            base.Dispose(); 
            StreamWriter?.Dispose();
        }


        protected override void WriteLog(string log)
        {
            FileManager.WriteText(StreamWriter, $"{log} \r\n");
        }
    }
}
