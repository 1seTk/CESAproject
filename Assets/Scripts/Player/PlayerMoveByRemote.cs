using UniRx;
using UniRx.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMoveByRemote : MonoBehaviour
{
    // ジャンプ力
    [SerializeField, Range(0,10)]
    private float _jumpPower;

    // ジャンプ距離
    [SerializeField, Range(0, 10)]
    private float _jumpDistance;

    // プレイヤーの状態取得用
    private PlayerCore _core;

    // 地面についているか
    private CheckGround _checkGR;

    // タッチ開始座標
    private Vector3 touchStartPos;

    // タッチ終了座標
    private Vector3 touchEndPos;


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

    private void Start()
    {
        _core = GetComponent<PlayerCore>();

        _checkGR = GetComponent<CheckGround>();

        // ジャンプ入力
        this.UpdateAsObservable()
            .Select(_ => Input.touchCount >= 2)
            .Subscribe(onJumpButtonSubject);

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
                    Debug.Log("single");
                    break;
                case 2:
                    if (_core.PlayerControllable.Value == true)
                    {
                        transform.position += new Vector3(0, _jumpPower, 0) * Time.deltaTime;
                        Debug.Log("double");
                    }
                    break;
            }
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

        Debug.Log("touchStartPos.y=" + touchStartPos.y);
        Debug.Log("touchEndPos.y=" + touchEndPos.y);


        Debug.Log("directionY=" + directionY);



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
                if (_core.PlayerControllable.Value == true && _checkGR.IsGround.Value == true)
                {
                    //transform.position += new Vector3(0, _jumpPower, _jumpPower) * Time.deltaTime;

                    _core.PlayerControllable.Value = false;


                    transform.DOJump(new Vector3(0, 0, transform.position.z + _jumpDistance), _jumpPower, 1, 1.5f)
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