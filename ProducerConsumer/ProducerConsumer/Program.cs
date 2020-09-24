using System;

namespace ProducerConsumer
{
	class Program
	{
		static void Main(string[] args)
		{
			Work work = new Work(5, 5);
			work.Run();
			Console.ReadKey();
			work.Stop();
			Console.WriteLine("Finished");
		}
	}
}
