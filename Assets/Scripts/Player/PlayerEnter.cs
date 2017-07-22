// E.Ogisu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;

public class PlayerEnter : MonoBehaviour {

    [SerializeField]
    private GameObject _targetObj;      // 着地点となるオブジェクト

    [SerializeField]
    private float _popPosY;             // taragetObjのどれだけ上に出現するか

    [SerializeField]
    private float _endSec;              // 着地完了までの時間

    [SerializeField]
    PlayerType _playerType;

    private bool _playAnim;             // プレイヤーがアニメーションしているか

	private Camera _playerCamera;		// プレイヤーのカメラ

	// プレイヤーが登場し終えたか
	public bool IsPlayerEnter
	{
		get { return !_playAnim; }
	}
  	//private ReactiveProperty<bool> isGround = new ReactiveProperty<bool>();
  
  	//// 移動しているか
    //public IReactiveProperty<bool> IsGround
    //{
    //	get { return isGround; }
    //}
  
    private void Awake()
    {
        // プレイヤーをアニメーション再生中
        _playAnim = true;

		// プレイヤーとカメラの親子関係を一旦リセット
		//_playerCamera = GetComponentInChildren<Camera>();
		//_playerCamera.transform.parent = null;

		// Playerの重力を無効にする(アニメーションに影響がないようにするため)
		GetComponent<Rigidbody>().useGravity = false;

        StartCoroutine(WaitStartCamera());
    }

    // Use this for initialization
    void Start ()
    {
		//// 設定された時間待機する
		//Observable.Return(Unit.Default)
		//    .Delay(TimeSpan.FromMilliseconds(_endSec * 1200))
		//    .Subscribe(_ =>
		//    {
		//        Debug.Log("active");
		//    });

		// player登場

		// SetPlayer();

    }

	void Update()
	{
		// Playerのアニメーションが終了したら
		if(_playAnim == false)
		{
			// 重力を有効にする
			GetComponent<Rigidbody>().useGravity = true;

			// カメラをプレイヤーに追従するようにする
			//_playerCamera.transform.parent = transform;
		}
	}

	private void SetPlayer()
    {
        // プレイヤーの座標を設定
        transform.position = _targetObj.transform.position + (Vector3.up * _popPosY);

        // プレイヤーのタイプに応じた登場処理を行う
        switch (_playerType)
        {
            case PlayerType.Normal:     // 通常
                transform.DOLocalMoveY(_targetObj.transform.position.y, _endSec).SetEase(Ease.OutBounce);

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad)
                    .OnKill(() =>
                    {
                        _playAnim = false;
                    });

                break;
            case PlayerType.Fat:        // 重め
                transform.DOLocalMoveY(_targetObj.transform.position.y, _endSec).SetEase(Ease.InExpo);

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad)
                    .OnKill(() =>
                    {
                        _playAnim = false;
                    });


                break;
            case PlayerType.Slim:       // 軽め
                transform.DOLocalMoveY(_targetObj.transform.position.y, _endSec).SetEase(Ease.InQuad);

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f,RotateMode.FastBeyond360).SetEase(Ease.OutQuad)
                    .OnKill(() =>
                    {
                        _playAnim = false;
                    });


                break;
        }
    }

	IEnumerator WaitStartCamera()
	{
		var cam = GetComponentInChildren<Camera>();
		while (cam != null)
		{
			yield return new WaitForEndOfFrame();
		}

		SetPlayer();
	}
}
