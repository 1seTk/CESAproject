// ---------------------------------------
// Brief : プレイヤーの移動
// 
// Date  : 2017/04/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
	[SerializeField, Range(0, 10)]
	private float m_speed;

	[SerializeField, Range(0, 10)]
	private float m_jumpPower = 10.0f;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var core = GetComponent<PlayerCore>();
		var input = GetComponent<IPlayerInput>();
		var col = GetComponent<PlayerCollision>();
		var cg = GetComponent<CheckGround>();
		var rb = GetComponent<Rigidbody>();

		// 移動処理
		input.IsMovingRP
			.Where(x => x == true)
			.Where(_ => core.PlayerControllable.Value == true)
			.Subscribe(x =>
			{
				transform.position += new Vector3(0, 0, m_speed) * Time.deltaTime;
				// rb.MovePosition(transform.position + new Vector3(0, 0, m_speed) * Time.deltaTime);
				// transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * m_speed / 10.0f, ForceMode.Impulse);
			});

		// ジャンプ処理
		input.OnJumpButtonObservable
			.Where(x => x == true)
			.Where(_ => core.PlayerControllable.Value == true)
			.Where(_ => cg.IsGround.Value == true )
			.Subscribe(_ =>
			{
				rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
			});

        // 衝突状態によって移動を制限する
        //this.UpdateAsObservable()
        //	.Subscribe(_ => core.PlayerControllable.Value = !col.IsHit.Value);

        // 移動処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(1) == true)
            .Where(_ => core.PlayerControllable.Value == true)
            .Subscribe(x =>
            {
                transform.position -= new Vector3(0, 0, m_speed) * Time.deltaTime;
                // rb.MovePosition(transform.position + new Vector3(0, 0, m_speed) * Time.deltaTime);
                // transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * m_speed / 10.0f, ForceMode.Impulse);
            });

    }
}
