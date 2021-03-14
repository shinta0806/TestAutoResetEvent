// ============================================================================
// 
// AutoResetEvent と CountdownEvent の速度差比較
// 
// ============================================================================

// ----------------------------------------------------------------------------
//
// ----------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestAutoResetEvent
{
	class Program
	{
		public static async Task Main(String[] args)
		{
			const Int32 NUM_TRIALS = 100;

			using AutoResetEvent autoResetEventMainToSub = new(false);
			using AutoResetEvent autoResetEventSubToMain = new(false);
			using CancellationTokenSource cancellationTokenSource = new();
			TestAutoResetEvent testAutoResetEvent = new(autoResetEventMainToSub, autoResetEventSubToMain, cancellationTokenSource.Token);
			Console.WriteLine("AutoResetEvent のテスト");
			Int32 autoResetEventStart = Environment.TickCount;
			Task autoResetEventTask = testAutoResetEvent.Invoke();
			for (Int32 i = 0; i < NUM_TRIALS; i++)
			{
				autoResetEventMainToSub.Set();
				autoResetEventSubToMain.WaitOne();
			}
			Console.WriteLine("所要時間：" + (Environment.TickCount - autoResetEventStart).ToString("#,0") + " [ms]");
			cancellationTokenSource.Cancel();
			autoResetEventMainToSub.Set();
			await autoResetEventTask;
			Console.WriteLine("カウント：" + testAutoResetEvent.Counter);
		}
	}
}
