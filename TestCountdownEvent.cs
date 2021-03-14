// ============================================================================
// 
// CountdownEvent の速度測定
// 
// ============================================================================

// ----------------------------------------------------------------------------
//
// ----------------------------------------------------------------------------

using System.Threading;

namespace TestAutoResetEvent
{
	public class TestCountdownEvent : TestSync
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public TestCountdownEvent(CountdownEvent countdownEventMainToSub, CountdownEvent countdownEventSubToMain, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			_countdownEventMainToSub = countdownEventMainToSub;
			_countdownEventSubToMain = countdownEventSubToMain;
		}

		// ====================================================================
		// protected メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// テスト
		// --------------------------------------------------------------------
		protected override void Test()
		{
			while (true)
			{
				_countdownEventMainToSub.Wait();
				_countdownEventMainToSub.Reset();
				if (_cancellationToken.IsCancellationRequested)
				{
					return;
				}

				Counter++;
				_countdownEventSubToMain.Signal();
			}
		}


		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// Main() からこちらへの通知
		private CountdownEvent _countdownEventMainToSub;

		// こちらから Main() への通知
		private CountdownEvent _countdownEventSubToMain;
	}
}
