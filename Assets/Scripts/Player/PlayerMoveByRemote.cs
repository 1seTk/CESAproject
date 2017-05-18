using UniRx;
using UniRx.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveByRemote : MonoBehaviour
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

    private void Start()
    {
        // ジャンプ入力
        this.UpdateAsObservable()
            .Select(_ => Input.touchCount >= 2)
            .Subscribe(onJumpButtonSubject);

    }

    private void Update()
    {
        OnTouchDown();
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

                    Debug.Log("double");

                    break;
            }
        }
    }
        //    // タッチ
    //    Touch touch;

    //    void Start()
    //    {
    //        if (Input.touchSupported)
    //        {
    //            Debug.Log("touch");
    //        }
    //    }


    //    void Update()
    //    {
    //        if (Input.touchCount > 0)
    //        {
    //            this.touch = Input.touches[0];

    //            if (this.touch.phase == TouchPhase.Began)
    //            {
    //                Debug.Log("Input.touchCount == " + Input.touchCount);
    //                switch (Input.touchCount)
    //                {
    //                    case 1: // 1本指でタッチ
    //                        Debug.Log("Single Touch");
    //                        break;

    //                    case 2: // 2本指でタッチ
    //                        Debug.Log("Double Touch");
    //                        break;

    //                    case 3: // 3本指でタッチ
    //                        Debug.Log("Triple Touch");
    //                        break;
    //                }
    //            }
    //        }
    //    }
}