// ============================================================================
// 
// AutoResetEvent の速度測定
// 
// ============================================================================

// ----------------------------------------------------------------------------
//
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAutoResetEvent
{
	public class TestAutoResetEvent
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public TestAutoResetEvent(AutoResetEvent autoResetEventMainToSub, AutoResetEvent autoResetEventSubToMain, CancellationToken cancellationToken)
		{
			_autoResetEventMainToSub = autoResetEventMainToSub;
			_autoResetEventToSubToMain = autoResetEventSubToMain;
			_cancellationToken = cancellationToken;
		}

		// ====================================================================
		// public プロパティー
		// ====================================================================

		public Int32 Counter { get; set; }

		// ====================================================================
		// public メンバー変数
		// ====================================================================

		// --------------------------------------------------------------------
		// テスト開始
		// --------------------------------------------------------------------
		public Task Invoke()
		{
			return Task.Run(Test);
		}

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// Main() からこちらへの通知
		private AutoResetEvent _autoResetEventMainToSub;

		// こちらから Main() への通知
		private AutoResetEvent _autoResetEventToSubToMain;

		// 中止用
		private CancellationToken _cancellationToken;

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// --------------------------------------------------------------------
		// テスト
		// --------------------------------------------------------------------
		private void Test()
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
	}
}
