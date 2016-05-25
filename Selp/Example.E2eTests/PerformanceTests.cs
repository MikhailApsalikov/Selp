namespace Example.E2eTests
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Net;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class PerformanceTests
	{
		private const string ServerLocation = "http://localhost:33332/";
		private const int RepeatTimes = 50;

		/*
		[TestMethod]
		public void Web2GetAllPerformanceTest()
		{
			var client = new WebClient();
			var result = Wrap(() => client.DownloadString($"{ServerLocation}/api/Region/"), RepeatTimes);
			Console.WriteLine($"Web 2.0 average test result is {result} ms");
		}

		[TestMethod]
		public void Web3GetAllPerformanceTest()
		{
			var client = new WebClient();
			var result = Wrap(() => client.DownloadString($"{ServerLocation}/semantic/Region/"), RepeatTimes);
			Console.WriteLine($"Web 3.0 average test result is {result} ms");
		}

		[TestMethod]
		public void Web2GetByIdPerformanceTest()
		{
			var client = new WebClient();
			var result = Wrap(() => client.DownloadString($"{ServerLocation}/api/User/admin"), RepeatTimes);
			Console.WriteLine($"Web 2.0 average test result is {result} ms");
		}

		[TestMethod]
		public void Web3GetByIdPerformanceTest()
		{
			var client = new WebClient();
			var result = Wrap(() => client.DownloadString($"{ServerLocation}/semantic/User/admin"), RepeatTimes);
			Console.WriteLine($"Web 3.0 average test result is {result} ms");
		}*/

		[TestMethod]
		public void FullPerformanceTest()
		{
			var client = new WebClient();

			client.DownloadString($"{ServerLocation}/api/Region/"); //тестовый прогон (чтобы избежать задержки 1 запуска)
			client.DownloadString($"{ServerLocation}/semantic/Region/");

			var result = Wrap(() => client.DownloadString($"{ServerLocation}/api/Region/"), RepeatTimes);
			Console.WriteLine($"Web 2.0 GetAll выполняется в среднем за {result} миллисекунд");
			result = Wrap(() => client.DownloadString($"{ServerLocation}/api/User/admin"), RepeatTimes);
			Console.WriteLine($"Web 2.0 GetById выполняется в среднем за {result} миллисекунд");
			result = Wrap(() => client.DownloadString($"{ServerLocation}/semantic/Region/"), RepeatTimes);
			Console.WriteLine($"Web 3.0 GetAll выполняется в среднем за {result} миллисекунд");
			result = Wrap(() => client.DownloadString($"{ServerLocation}/semantic/User/admin"), RepeatTimes);
			Console.WriteLine($"Web 3.0 GetById выполняется в среднем за {result} миллисекунд");
		}

		private double Wrap(Action action, int repeatTimes)
		{
			var results = new long[repeatTimes];

			for (var i = 0; i < repeatTimes; i++)
			{
				Stopwatch sw = Stopwatch.StartNew();
				action();
				sw.Stop();
				results[i] = sw.ElapsedMilliseconds;
			}

			return results.Sum()/(double) repeatTimes;
		}
	}
}