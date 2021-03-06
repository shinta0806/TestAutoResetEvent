// ============================================================================
// 
// AutoResetEvent の速度測定
// 
// ============================================================================

// ----------------------------------------------------------------------------
//
// ----------------------------------------------------------------------------

using System.Threading;

namespace TestAutoResetEvent
{
	public class TestAutoResetEvent : TestSync
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public TestAutoResetEvent(AutoResetEvent autoResetEventMainToSub, AutoResetEvent autoResetEventSubToMain, CancellationToken cancellationToken)
				: base(cancellationToken)
		{
			_autoResetEventMainToSub = autoResetEventMainToSub;
			_autoResetEventToSubToMain = autoResetEventSubToMain;
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
				_autoResetEventMainToSub.WaitOne();
				if (_cancellationToken.IsCancellationRequested)
				{
					return;
				}

				Counter++;
				_autoResetEventToSubToMain.Set();
			}
		}

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// Main() からこちらへの通知
		private AutoResetEvent _autoResetEventMainToSub;

		// こちらから Main() への通知
		private AutoResetEvent _autoResetEventToSubToMain;
	}
}
