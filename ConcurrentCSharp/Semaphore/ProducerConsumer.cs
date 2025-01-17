﻿using System;
using System.Threading;


// todo 3: Check methods Producer::produce() and Consumer::consume(). Using defined semaphores, protect shared memory.
namespace ProducerConsumer
{
    public class PCBuffer
    {
        public int[] buffer;
        public int emptyIndex { get; set; }

        public PCBuffer(int size)
        {
            buffer = new int[size];
            emptyIndex = 0;
        }
        public void write(int pid)
        {
            buffer[emptyIndex]++;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Out.WriteLine("[Producer {0}] wrote {1} at index {2}", pid.ToString(), buffer[emptyIndex].ToString(), emptyIndex.ToString());
            Console.ForegroundColor = ConsoleColor.Black;

            emptyIndex = (emptyIndex + 1) % buffer.Length;
        }
        public int read(int cid)
        {
            int readIndex = (emptyIndex + buffer.Length - 1) % buffer.Length;
            int result = buffer[readIndex];
            Console.Out.WriteLine("[Consumer {0}] read {1} from index {2} ", cid.ToString(), result.ToString() , readIndex.ToString());
            return result;
        }
    }
    public class Producer
    {
        private int minTime { get; set; }
        private int maxTime { get; set; }
        private int id { get; set; }
        private PCBuffer buffer;
        private Semaphore producerSemaphore, consumerSemaphore;

        public Producer(int id, int min, int max, PCBuffer buf, Semaphore psem, Semaphore csem)
        {
            this.id = id;
            this.minTime = min;
            this.maxTime = max;
            this.buffer = buf;
            this.producerSemaphore = psem;
            this.consumerSemaphore = csem;
        }

        public void produce()
        {
            Thread.Sleep(new Random().Next(minTime, maxTime));
            int data = new Random().Next();

            this.buffer.write(this.id);
        }
        public void MultiProduce(Object n)
        {
            int num = (int)n;
            for (int i = 0; i < num; i++)
                this.produce();
        }
    }
    public class Consumer
    {
        private int minTime { get; set; }
        private int maxTime { get; set; }
        private int id { get; set; }
        private PCBuffer buffer;
        private Semaphore producerSemaphore, consumerSemaphore;

        public Consumer(int id, int min, int max, PCBuffer buf, Semaphore psem, Semaphore csem)
        {
            this.id = id;
            this.minTime = min;
            this.maxTime = max;
            this.buffer = buf;
            this.producerSemaphore = psem;
            this.consumerSemaphore = csem;
        }

        public void consume()
        {
            Thread.Sleep(new Random().Next(minTime, maxTime));

            int data = this.buffer.read(this.id);
        }
        public void MultiConsume(Object n)
        {
            int num = (int)n;
            for (int i = 0; i < num; i++)
                this.consume();
        }
    }
}
