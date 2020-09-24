using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProducerConsumer
{
	class Consumer
	{
		List<int> buffer;
		Mutex Mutex;
		int id;
		bool check = true;
		Thread thread;

		public Consumer(List<int> buffer, Mutex mutex, int id)
		{
			Mutex = mutex;
			this.buffer = buffer;
			this.id = id;
			thread = new Thread(Consume);
			thread.Start();
		}

		public void Consume()
		{
			while (check)
			{
				Mutex.WaitOne();
				if (buffer.Count != 0)
				{
					int item = buffer[buffer.Count - 1];
					buffer.Remove(item);
					Console.WriteLine("Consumer {0} pop item: {1}", id, item);
				}
				Mutex.ReleaseMutex();
				Thread.Sleep(100);
			}
		}

		public void Stop()
		{
			check = false;
			thread.Join();
		}

		public bool IsAlive()
		{
			return thread.IsAlive;
		}
	}
}
