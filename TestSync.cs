﻿// ============================================================================
// 
// 速度測定の基底クラス
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
	public abstract class TestSync
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public TestSync(CancellationToken cancellationToken)
		{
			_cancellationToken = cancellationToken;
		}

		// ====================================================================
		// public プロパティー
		// ====================================================================

		// カウンター
		public Int32 Counter { get; set; }

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// テスト開始
		// --------------------------------------------------------------------
		public Task Invoke()
		{
			return Task.Run(Test);
		}

		// --------------------------------------------------------------------
		// カウンター表示
		// --------------------------------------------------------------------
		public void ShowCounter()
		{
			Console.WriteLine("カウント：" + Counter.ToString("#,0") + "（同期回数は " + (Counter * 2).ToString("#,0") + "）");
		}

		// ====================================================================
		// protected メンバー変数
		// ====================================================================

		// 中止用
		protected CancellationToken _cancellationToken;

		// ====================================================================
		// protected メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// テスト
		// --------------------------------------------------------------------
		protected abstract void Test();
	}
}
