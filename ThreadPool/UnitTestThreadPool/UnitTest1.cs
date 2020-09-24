using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using ThreadPool;

namespace UnitTestThreadPool
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethodOneTask()
		{
			ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool(new Random(), 4);
			MyTask<String> task = new MyTask<String>(() => "Hello");
			threadPool.Enqueue(task);
			threadPool.Start();
			Thread.Sleep(1000);
			threadPool.Dispose();
			Assert.AreEqual("Hello", task.Result());
		}

		[TestMethod]
		public void TestMethodSomeTask()
		{
			ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool(new Random(), 4);
			MyTask<int> task1 = new MyTask<int>(() => 2 + 2);	
			MyTask<int> task2 = new MyTask<int>(() => { int x = 0; return 1 / x; });				
			MyTask<String> task3 = new MyTask<String>(() => "Task3");

			threadPool.Enqueue(task1);
			threadPool.Enqueue(task2);
			threadPool.Enqueue(task3);
			threadPool.Start();
			Thread.Sleep(1000);
			threadPool.Dispose();
			Assert.AreEqual(4, task1.Result());
			Assert.ThrowsException<AggregateException>(() => task2.Execute());
			Assert.AreEqual("Task3", task3.Result());
		}

		[TestMethod]
		public void TestMethodContinueWith()
		{
			ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool(new Random(), 4);
			var task1 = new MyTask<int>(() => 2 + 2);
			var task2 = task1.ContinueWith(y => y + 1);
			var task3 = task2.ContinueWith(y => y * 2);
			threadPool.Enqueue(task1);
			threadPool.Enqueue(task2);
			threadPool.Enqueue(task3);
			threadPool.Start();
			Thread.Sleep(1000);
			threadPool.Dispose();
			Assert.AreEqual(4, task1.Result());
			Assert.AreEqual(5, task2.Result());
			Assert.AreEqual(10, task3.Result());
		}

		[TestMethod]
		public void TestMethodManyPools()
		{
			ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool(new Random(), 20);
			var task1 = new MyTask<int>(() => 2 + 2);
			var task2 = task1.ContinueWith(y => y + 1);
			var task3 = task2.ContinueWith(y => y * 2);
			threadPool.Enqueue(task1);
			threadPool.Enqueue(task2);
			threadPool.Enqueue(task3);
			threadPool.Start();
			Assert.AreEqual(20, threadPool.ThreadAmount());
			Thread.Sleep(1000);
			threadPool.Dispose();
			Assert.AreEqual(0, threadPool.ThreadAmount());
		}
	}
}
