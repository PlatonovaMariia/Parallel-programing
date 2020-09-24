using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
	class Program
	{
		static List<String> PonyNames = new List<String>() { "Twilight Sparkle", "Rainbow Dash", "Rarity", "Fluttershy", "Pinkie Pie", "Applejack" };
		static char Heart = '\u2665';
		static Random Random = new Random();

		static void Main(string[] args)
		{
			Console.WriteLine("Pony tasks!");
			ThreadPool threadPool = new ThreadPool(Random, 4);

			int taskCount = 20;
			List<MyTask<String>> tasks = new List<MyTask<String>>();
			tasks.Add(new MyTask<String>(() => GetPonyName()));
			for (int i = 1; i < taskCount; i++)
			{
				bool check = Random.Next(0, 2).Equals(1);
				MyTask<String> task;
				if (check)
				{
					task = new MyTask<String>(() => GetPonyName());
				}
				else
				{
					int indexPreviousTask = Random.Next(0, tasks.Count);
					task = (MyTask<String>)tasks[indexPreviousTask].ContinueWith(y => GetPonyName(y));
				}
				tasks.Add(task);
			}

			foreach(MyTask<String> task in tasks)
			{
				threadPool.Enqueue(task);
			}

			threadPool.Start();
			Thread.Sleep(1000);
			threadPool.Dispose();
			Console.WriteLine("So many ponies!");
			Console.ReadKey();
		}

		public static String GetPonyName()
		{
			int indexPony = Random.Next(0, PonyNames.Count);
			return PonyNames[indexPony];
		}

		public static String GetPonyName(String previousName)
		{
			int indexPony = Random.Next(0, PonyNames.Count);
			if (PonyNames.Contains(previousName))
			{
				return String.Format("{0} + {1} = {2}", previousName, PonyNames[indexPony], Heart);
			}
			else
			{
				return String.Format("You + {0} = {1}", PonyNames[indexPony], Heart);
			}
		}
	}
}
