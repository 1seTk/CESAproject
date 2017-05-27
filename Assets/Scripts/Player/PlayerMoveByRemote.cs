using UniRx;
using UniRx.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.SceneManagement;
using System;

public class PlayerMoveByRemote : MonoBehaviour, IPlayerInput
{
    // タッチ開始座標
    private float _touchStartPosY;

    // タッチ終了座標
    private float _touchEndPosY;

    private int _touchTime = 0;

    private bool _jumpFlg = false;

    private bool _flickFlg = false;

    private bool _advanceFlg = false;

    [Range(5,100)]
    public int _delayFrame = 20;

    private CheckGround _checkGround;

    private bool _oldGroundValue = false;

    [Range(0,10)]
    public float _adPower = 3f;

    [Range(1, 10)]
    public float _jumpTime = 1;

	// インタフェース実装部分 =====

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

	private ReactiveProperty<bool> isMovingRP = new ReactiveProperty<bool>();

	// 移動しているか
	public IReactiveProperty<bool> IsMovingRP
	{
		get { return isMovingRP; }
	}

	private void Start()
	{
        _checkGround = GetComponent<CheckGround>();

		// ジャンプ入力
		this.UpdateAsObservable()
			.Select(_ => _jumpFlg == true)
			.Subscribe(onJumpButtonSubject);


		// 移動入力
		this.UpdateAsObservable()
			.Select(_ => _advanceFlg == true)
			.Subscribe(x =>
			{
				isMovingRP.SetValueAndForceNotify(x);
			});
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        foreach (Touch t in Input.touches)
        {
            switch (t.phase)
            {
                case TouchPhase.Began:
                    // タッチ開始座標を代入
                    _touchStartPosY = t.position.y;

                    Debug.Log("Begin");
                    break;
                case TouchPhase.Stationary:
                    // タッチ時間計測を開始
                    _touchTime++;
                    Debug.Log("Stationaly");
                    
                    // タッチ時間が一定フレームを超えたら前進開始
                    if (_touchTime > _delayFrame && _checkGround.IsGround.Value == true)
                    {
                        _advanceFlg = true;
                    }
                    break;
                case TouchPhase.Ended:
                    // タッチ終了座標を代入
                    _touchEndPosY = t.position.y;

                    Debug.Log("StartPos" + _touchStartPosY);

                    Debug.Log("EndPos" + _touchEndPosY);

                    // 移動していない場合ジャンプを実行
                    if (_advanceFlg == false)
                    {                        
                        _jumpFlg = true;
                        Observable.NextFrame().Subscribe(_ =>
                        {
                            _jumpFlg = false;
                        });
                    }
                    
                    // 指が離れた時に前進移動を停止
                    if ((_advanceFlg == true && _checkGround.IsGround.Value) == true)
                    {
                        _advanceFlg = false;
                    }

                    if (30 < Math.Abs(_touchEndPosY) - Mathf.Abs(_touchStartPosY))
                    {
                        // スワイプしている場合
                        //_advanceFlg = true;
                        if ((_checkGround.IsGround.Value) == true)
                        transform.DOMoveZ(transform.position.z + _adPower, _jumpTime);
                    }
                    // タッチ時間をリセット
                    _touchTime = 0;
                    break;
            }
        }
        if (_checkGround.IsGround.Value != _oldGroundValue)
        {
            _advanceFlg = false;
        }

        _oldGroundValue = _checkGround.IsGround.Value;
    }

}