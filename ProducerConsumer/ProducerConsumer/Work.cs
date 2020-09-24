using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProducerConsumer
{
	public class Work
	{
		int numOfProducer;
		int numOfConsumer;
		List<int> buffer = new List<int>();
		Mutex mutex = new Mutex();

		List<Producer> producers = new List<Producer>();
		List<Consumer> consumers = new List<Consumer>();

		public Work(int numOfProducer, int numOfConsumer)
		{
			this.numOfProducer = numOfProducer;
			this.numOfConsumer = numOfConsumer;
		}

		public void Run()
		{
			if (numOfProducer <= 0 || numOfConsumer <= 0)
			{
				throw new ArgumentOutOfRangeException("Invalid number of producers or consumers");
			}
			else
			{
				for (int i = 0; i < numOfProducer; i++)
				{
					producers.Add(new Producer(buffer, mutex, i));
				}
				for (int i = 0; i < numOfConsumer; i++)
				{
					consumers.Add(new Consumer(buffer, mutex, i));
				}
			}
		}

		public void Stop()
		{
			producers.ForEach(i => i.Stop());
			consumers.ForEach(i => i.Stop());
		}

		public bool IsAlive()
		{
			foreach (Producer item in producers)
			{
				if (item.IsAlive())
				{
					return false;
				}
			}

			foreach (Consumer item in consumers)
			{
				if (item.IsAlive())
				{
					return false;
				}
			}

			return true;
		}
	}
}
