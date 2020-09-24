using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadPool
{
	public interface IMyTask
	{
		public bool IsCompleted();
		public void Execute();
	}

	public interface IMyTask<TResult> : IMyTask
	{
		public TResult Result();
		public IMyTask<TNewResult> ContinueWith <TNewResult>(Func<TResult, TNewResult> function);
	}

	public class MyTask<TResult> : IMyTask<TResult>
	{
		TResult result;
		Func<TResult> function;
		bool completed;

		public MyTask(Func<TResult> function)
		{
			this.function = function;
			completed = false;
		}

		public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> function)
		{
			return new MyTask<TNewResult>(() =>
			{
				while (!IsCompleted())
				{
					Thread.Sleep(10);
				};
				return function(Result());
			});
		}

		public bool IsCompleted()
		{
			return completed;
		}

		public TResult Result()
		{
			if (!completed)
			{
				try
				{
					result = function();
				}
				catch (Exception e)
				{
					throw new AggregateException(e);
				}
				completed = true;
			}
			return result;
		}

		public void Execute()
		{
			TResult result = Result();
			Console.WriteLine(result);
		}
	}
}
