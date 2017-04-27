// E.Ogisu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEnter : MonoBehaviour {

    [SerializeField]
    private GameObject _targetObj;      // 着地点となるオブジェクト

    [SerializeField]
    private float _popPosY;             // taragetObjのどれだけ上に出現するか

    [SerializeField]
    private float _endSec;              // 着地完了までの時間

    [SerializeField]
    PlayerType _playerType;

	// Use this for initialization
	void Start () {
        SetPlayer();
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

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

                break;
            case PlayerType.Fat:        // 重め
                transform.DOLocalMoveY(_targetObj.transform.position.y, _endSec).SetEase(Ease.InExpo);

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

                //transform.DOPunchScale(new Vector3(0.5f, -0.5f, 0.5f), 3f,0);

                break;
            case PlayerType.Slim:       // 軽め
                transform.DOLocalMoveY(_targetObj.transform.position.y, _endSec).SetEase(Ease.InQuad);

                transform.DOLocalRotate(new Vector3(0f, 1800, 0f), _endSec + 0.5f,RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

                break;
        }
    }
}
