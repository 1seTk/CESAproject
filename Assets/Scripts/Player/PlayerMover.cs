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
using DG.Tweening;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
	[SerializeField, Range(0, 10)]
	private float m_speed;

	[SerializeField, Range(0, 10)]
	private float m_jumpPower = 10.0f;

	// 正面にギミックが存在する時に進めなくする用
	private RaycastHit m_hit = new RaycastHit();

	// 正面方向を記憶しておく(回転してもいいように)
	private Vector3 m_forward;

    private float m_maxSpeed;
    private float m_maxSpeedSqr;

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
		var enter = GetComponent<PlayerEnter>();
        var goal = GameObject.Find("GoalEx").GetComponent<GoalDetector>();

		// 正面方向を記憶しておく
		m_forward = transform.parent.forward;

        // 最大速度は移動速度の1.5倍
        m_maxSpeed = m_speed * 1.5f;

        // 最大速度の2乗をあらかじめ計算しておく
        m_maxSpeedSqr = m_maxSpeed * m_maxSpeed;

        // 移動処理
        input.IsMovingRP
			.Where(x => x == true)
			.Where(_ => core.PlayerControllable.Value == true)
            .Where(_ => core.Invincible != true)
			.Subscribe(x =>
			{
				transform.position += new Vector3(0, 0, m_speed) * Time.deltaTime;
				// rb.MovePosition(transform.position + new Vector3(0, 0, m_speed) * Time.deltaTime);
				// transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * m_speed / 10.0f, ForceMode.Impulse);
			});

        // ジャンプ処理
        input.OnJumpButtonObservable
            .DistinctUntilChanged()
            .Where(_ => enter.IsPlayerEnter == true)
            //.Where(_ => core.PlayerControllable.Value == true)
            .Where(_ => cg.IsGround.Value == true)
            .Where(_ => core.Invincible != true)
			.Subscribe(_ =>
			{
				rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
			});

		// 衝突相手の情報を更新する
		this.UpdateAsObservable()
			.Where(_ => enter.IsPlayerEnter)
			.Subscribe(_ =>
			{
				// Player以外と当たっていたら
				if (Physics.BoxCast(transform.position, (Vector3.one * 0.8f) * transform.lossyScale.x * 0.5f, m_forward, Quaternion.identity, 0.25f, ~LayerMask.GetMask("Player")))
				{
					// 進めなくする
					core.PlayerControllable.Value = false;
				}
				else
				{
					// 進めるようにする
					core.PlayerControllable.Value = true;
				}
			});

        this.UpdateAsObservable()
            .Where(_ => transform.forward != m_forward)
            .Where(_ => enter.IsPlayerEnter == true)
            .Subscribe(_ =>
            {
                transform.DORotateQuaternion(Quaternion.identity, 0.2f);
            });

        this.FixedUpdateAsObservable()
            .Where(_ => transform.rotation.z < 0.1f)
            .Where(_ => transform.rotation.z > -0.1f)
            .Subscribe(_ =>
            {
                transform.DORotateQuaternion(Quaternion.identity, 0.2f);
            });

        // 移動速度制限処理
        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                Vector3 v = rb.velocity;
                if (v.sqrMagnitude > m_maxSpeedSqr)
                {
                    v = rb.velocity - (v.normalized * m_maxSpeed);
                    rb.velocity -= v;
                }
            });
    }
}
