﻿using System;
using System.IO;
using System.Text;

namespace CodeCraft.Logger.ProducerConsumer
{
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
        readonly StringBuilder strBuilder = new StringBuilder();
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
        private void DisposeCurrentStream()
        {
            StreamWriter?.Close();
            StreamWriter?.Dispose();
        }
        private void FileLogProducerConsumer_FilePathEvent(object sender, FilePathEventArgs e)
        {
            DisposeCurrentStream();
            StreamWriter =  new StreamWriter(FileLogPath, true) { AutoFlush = true };

        }

        private void SendFilePathEvent()
        {
            InitializeMode = true;
            var args = new FilePathEventArgs(fileLogPath);
            InitializeAsyncResult = FilePathEvent.BeginInvoke(this, args, (result) => FilePathSettingsEnd(), null); 
        }
        void FilePathSettingsEnd()
        {
            InitializeMode = false;
        }

        ~FileLogProducerConsumer()
        {
            DisposeCurrentStream();
        }

        protected override void WriteLog(string log)
        {
            
            StreamWriter.WriteLine(log);
        }
    }
}
