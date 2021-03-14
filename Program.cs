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
			// 繰り返し回数
			// 1 回の繰り返しでメイン→サブ、サブ→メインの 2 回同期が発生する
			const Int32 NUM_REPEATS = 5000000;

			// AutoResetEvent 準備
			using AutoResetEvent autoResetEventMainToSub = new(false);
			using AutoResetEvent autoResetEventSubToMain = new(false);
			using CancellationTokenSource autoResetEventCancellation = new();
			TestAutoResetEvent testAutoResetEvent = new(autoResetEventMainToSub, autoResetEventSubToMain, autoResetEventCancellation.Token);

			// AutoResetEvent テスト
			Console.WriteLine("AutoResetEvent のテスト");
			Int32 autoResetEventStart = Environment.TickCount;
			Task autoResetEventTask = testAutoResetEvent.Invoke();
			for (Int32 i = 0; i < NUM_REPEATS; i++)
			{
				autoResetEventMainToSub.Set();
				autoResetEventSubToMain.WaitOne();
			}
			Console.WriteLine("所要時間：" + (Environment.TickCount - autoResetEventStart).ToString("#,0") + " [ms]");

			// AutoResetEvent 後始末
			autoResetEventCancellation.Cancel();
			autoResetEventMainToSub.Set();
			await autoResetEventTask;
			testAutoResetEvent.ShowCounter();

			// CountdownEvent 準備
			using CountdownEvent countdownEventMainToSub = new(1);
			using CountdownEvent countdownEventSubToMain = new(1);
			using CancellationTokenSource countdownEventCancellation = new();
			TestCountdownEvent testCountdownEvent = new(countdownEventMainToSub, countdownEventSubToMain, countdownEventCancellation.Token);

			// CountdownEvent テスト
			Console.WriteLine(String.Empty);
			Console.WriteLine("CountdownEvent のテスト");
			Int32 countdownEventStart = Environment.TickCount;
			Task countdownEventTask = testCountdownEvent.Invoke();
			for (Int32 i = 0; i < NUM_REPEATS; i++)
			{
				countdownEventMainToSub.Signal();
				countdownEventSubToMain.Wait();
				countdownEventSubToMain.Reset();
			}
			Console.WriteLine("所要時間：" + (Environment.TickCount - countdownEventStart).ToString("#,0") + " [ms]");

			// CountdownEvent 後始末
			countdownEventCancellation.Cancel();
			countdownEventMainToSub.Signal();
			await countdownEventTask;
			testCountdownEvent.ShowCounter();
		}
	}
}
