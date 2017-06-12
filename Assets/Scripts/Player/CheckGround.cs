// ---------------------------------------
// Brief : 接地判定
// 
// Date  : 2017/05/09
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class CheckGround : MonoBehaviour
{
	// 衝突判定を取る距離
	//[SerializeField]
	//private float m_hitDistance;

	private ReactiveProperty<bool> isGroundRP = new ReactiveProperty<bool>(false);

	/// <summary>
	/// 接地しているかどうか
	/// </summary>
	public IReactiveProperty<bool> IsGround
	{
		get { return isGroundRP; }
	}

	void Start ()
	{
		this.OnTriggerStayAsObservable()
			.Subscribe(_ => isGroundRP.Value = true);

		this.OnTriggerExitAsObservable()
			.Subscribe(_ => isGroundRP.Value = false);
	}
}
