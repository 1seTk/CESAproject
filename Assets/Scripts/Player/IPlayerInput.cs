// ---------------------------------------
// Brief : プレイヤーの入力インターフェース
// 
// Date  : 2017/05/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;

public interface IPlayerInput
{
	/// <summary>
	/// ジャンプボタンが押されているかどうか
	/// </summary>
	IObservable<bool> OnJumpButtonObservable { get; }

	/// <summary>
	/// 移動ボタンが押されているかどうか
	/// </summary>
	IReactiveProperty<bool> IsMovingRP { get; }
}
