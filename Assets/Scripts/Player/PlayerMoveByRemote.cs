using UniRx;
using UniRx.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.SceneManagement;

public class PlayerMoveByRemote : MonoBehaviour
{
    // ジャンプ力
    [SerializeField, Range(0,10)]
    private float _jumpPower;

    // ジャンプ距離
    [SerializeField, Range(0, 10)]
    private float _jumpDistance;

    // アニメーション時間
    [SerializeField, Range(0, 10)]
    private float _duration;

    // プレイヤーの状態取得用
    private PlayerCore _core;

    // 地面についているか
    private CheckGround _checkGR;

    // タッチ開始座標
    private Vector3 touchStartPos;

    // タッチ終了座標
    private Vector3 touchEndPos;

    [Range(0,1.0f)]
    public float _touchDelayCnt;

    // ジャンプ入力監視サブジェクト
    private Subject<bool> onJumpButtonSubject = new Subject<bool>();

    bool jumpFlg = false;

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

    private void Start()
    {
        _core = GetComponent<PlayerCore>();

        _checkGR = GetComponent<CheckGround>();

        // ジャンプ入力
        //this.UpdateAsObservable()
        //    .Select(_ => Input.touchCount >= 2 && jumpFlg == true)
        //    .Subscribe(onJumpButtonSubject);

    }

    private void Update()
    {
        OnTouchDown();

        Flick();
    }
    void OnTouchDown()
    {
        if(0 < Input.touchCount)
        {
            switch (Input.touchCount)
            {
                case 1:
                    if (_touchDelayCnt <= 1.5f && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        Debug.Log("single");
                        if (_core.PlayerControllable.Value == true)
                        {
                            _core.PlayerControllable.Value = false;

                            transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z), _jumpPower, 1, 1).OnComplete(
                                () =>
                                {
                                    _core.PlayerControllable.Value = true;

                                    Debug.Log("jumpEnd");
                                }
                                );
                        }
                    }
                    break;
            }
        }
        else
        {
            _touchDelayCnt = 0;
        }
        if (Input.touchCount == 1)
        {
            _touchDelayCnt+=0.1f;
        }
    }

    // フリック操作
    void Flick()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                            Input.mousePosition.y,
                            Input.mousePosition.z);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);

            GetDirection();
        }
    }

    void GetDirection()
    {
        float directionY = Mathf.Abs(touchEndPos.y) - Mathf.Abs(touchStartPos.y);
        string Direction = null;

        if (30 < directionY)
        {
            //上向きにフリック
            Direction = "up";
        }
        else
        {
            //タッチを検出
            Direction = "touch";
        }

        switch (Direction)
        {
            case "up":
                //上フリックされた時の処理
                if (_core.PlayerControllable.Value == true /*&& _checkGR.IsGround.Value == true*/)
                {
                    _core.PlayerControllable.Value = false;

                    transform.DOJump(new Vector3(0, 0, transform.position.z + _jumpDistance), _jumpPower, 1, _duration)
                        .OnComplete(()=> {
                            _core.PlayerControllable.Value = true;

                            Debug.Log("animend");

                        });
                    
                    Debug.Log("flick");
                }

                break;
        }

    }
}