using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CodeCraft.Logger.ProducerConsumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCraft.Logger.NetFramework.UnitTests
{
    class MyModel
    {
        public string FileName { get; set; }
        public string Text { get; set; }
    }

    internal class MyProducerConsumer : ProducerConsumer<MyModel>
    {
        public MyProducerConsumer() : base("MyThread") { StartConsumerTask(); }
        protected override void Process(MyModel data)
        {
            System.IO.File.WriteAllText(data.FileName, data.Text);
        }
    }
    [TestClass]
    public class ProducerConsumerTest 
    {
        [TestMethod]
        public void SimpleTest()
        { 
            var allData = new List<MyModel>();
            for (int i = 0; i < 100; i++)
                allData.Add(new MyModel
                {
                    FileName = $".//{i}.txt",
                    Text = DateTime.Now.Ticks.ToString ()
                });

            using (var producer = new MyProducerConsumer())
                allData.ForEach(m => producer.Produce(m));

        
        }
    
    }

}
