using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProducerConsumer
{
	class Producer
	{
		List<int> buffer;
		Mutex Mutex;
		int id;
		Random random = new Random();
		bool check = true;
		Thread thread;

		public Producer(List<int> buffer, Mutex mutex, int id)
		{
			Mutex = mutex;
			this.buffer = buffer;
			this.id = id;
			thread = new Thread(Produce);
			thread.Start();
		}

		public void Produce()
		{
			while (check)
			{
				Mutex.WaitOne();
				int item = random.Next(1, 1000);
				buffer.Add(item);
				Console.WriteLine("Producer {0} push item: {1}", id, item);
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
