using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadPool
{
	class Queue
	{
		List<IMyTask> tasks = new List<IMyTask>();
		Mutex mutex;

		public Queue(Mutex mutex)
		{
			this.mutex = mutex;
		}

		public void Enqueue(IMyTask task)
		{
			mutex.WaitOne();

			tasks.Add(task);

			mutex.ReleaseMutex();
		}

		public IMyTask Dequeue()
		{
			mutex.WaitOne();

			if (tasks.Count > 0)
			{
				var task = tasks[0];
				tasks.RemoveAt(0);

				mutex.ReleaseMutex();
				return task;
			}
			else
			{
				mutex.ReleaseMutex();
				return null;
			}
		}

		public bool IsEmpty()
		{
			mutex.WaitOne();

			if (tasks.Count == 0)
			{ 
				mutex.ReleaseMutex();
				return true;
			}
			else
			{
				mutex.ReleaseMutex();
				return false;
			}
		}
	}
}
