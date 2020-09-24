using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProducerConsumer;

namespace UnitTestProducerConsumer
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestZero()
		{
			Work work = new Work(0, 0);
			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => work.Run());
		}

		[TestMethod]
		public void TestNegative1()
		{
			Work work = new Work(-5, 5);
			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => work.Run());
		}

		[TestMethod]
		public void TestNegative2()
		{
			Work work = new Work(5, -5);
			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => work.Run());
		}

		[TestMethod]
		public void TestNegative3()
		{
			Work work = new Work(-5, -5);
			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => work.Run());
		}

		[TestMethod]
		public void TestNormalValue1()
		{
			Work work = new Work(10, 1);
			Assert.AreEqual(true, work.IsAlive());
		}

		[TestMethod]
		public void TestNormalValue2()
		{
			Work work = new Work(10, 1);
			Assert.AreEqual(true, work.IsAlive());
		}

		[TestMethod]
		public void TestNormalValue3()
		{
			Work work = new Work(5, 4);
			Assert.AreEqual(true, work.IsAlive());
		}
	}
}

