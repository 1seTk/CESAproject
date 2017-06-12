// ---------------------------------------
// Brief : プレイヤーの入力
// 
// Date  : 2017/05/09
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
	// ジャンプ入力監視サブジェクト
	private Subject<bool> onJumpButtonSubject = new Subject<bool>();

	/// <summary>
	/// ジャンプボタンが押されているか
	/// </summary>
	public IObservable<bool> OnJumpButtonObservable
	{
		get
		{
			return onJumpButtonSubject.AsObservable();
		}
	}

	private ReactiveProperty<bool> isMovingRP = new ReactiveProperty<bool>(false);

	// 移動しているか
	public IReactiveProperty<bool> IsMovingRP
	{
		get { return isMovingRP; }
	}

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// ジャンプ入力
		this.UpdateAsObservable()
			.Select(_ => Input.GetButtonDown("Jump"))
			.Subscribe(onJumpButtonSubject);

		// 移動入力
		this.UpdateAsObservable()
			.Select(_ => Input.GetButton("Fire1"))
			.Subscribe(x =>
			{
				isMovingRP.SetValueAndForceNotify(x);
			});
	}

}
