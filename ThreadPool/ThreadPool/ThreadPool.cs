using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
	public class ThreadPool : IDisposable
	{
		Random random;
		int threadCount;
		Dictionary<Thread, Queue> data = new Dictionary<Thread, Queue>();
		Mutex mutex;
		bool isDisposed = false;

		public ThreadPool(Random random, int n)
		{
			mutex = new Mutex();
			this.random = random;

			threadCount = n;
			for (int i = 0; i < threadCount; i++)
			{
				data.Add(new Thread(Run), new Queue(mutex));
			}
		}

		public void Run()
		{
			while (!isDisposed)
			{
				IMyTask task = data[Thread.CurrentThread].Dequeue();
				if (task == null)
				{
					int indexStolenThread = random.Next(0, data.Count);
					Thread stolenThread = data.ElementAt(indexStolenThread).Key;

					if (!data.ElementAt(indexStolenThread).Value.IsEmpty())
					{
						IMyTask stolenTask = data.ElementAt(indexStolenThread).Value.Dequeue();
						if (stolenTask != null)
						{
							data[Thread.CurrentThread].Enqueue(stolenTask);
							Thread.Sleep(10);
						}
					}
				}
				else
				{
					task.Execute();
				}
			}
		}

		public void Enqueue<TResult>(IMyTask<TResult> a)
		{
			if (data.Count != 0)
			{
				int indexToAddTask = random.Next(0, data.Count);
				data.Values.ToList()[indexToAddTask].Enqueue(a);
			}
		}


		public void Start()
		{
			isDisposed = false;
			foreach (Thread thread in data.Keys)
			{
				thread.Start();
			}
		}

		public void Finish()
		{
			isDisposed = true;
			foreach (Thread thread in data.Keys)
			{
				thread.Join();
			}
		}

		public void Dispose()
		{
			Finish();
			data.Clear();
		}

		public int ThreadAmount()
		{
			return data.Keys.ToList().Count;
		}

		~ThreadPool()
		{
			Dispose();
		}
	}
}
