using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AlphaCamera : MonoBehaviour {

    [SerializeField]
    GoalDetector m_goal;

	// Use this for initialization
	void Start () {
        // 半透明にすりゅ
        this.OnTriggerStayAsObservable()
            .Where(x => LayerMask.LayerToName(x.gameObject.layer) != "Ignore Alpha")
            .Where(x => x.GetComponent<PlayerCore>() == null)
            // .Where(x => x.GetComponent<Renderer>().material.GetTag("RenderType", true) != "Transparent")
            .Subscribe(x =>
            {
                // 距離をとる
                Vector3 distance = x.transform.position - transform.position;
                // 色をとる
                var color = x.GetComponent<Renderer>().material.color;
                // 距離を0~1で正規化(6.0fはプレイヤーとの距離)
                float alpha = distance.z / 6.0f;

                // マテリアルの参照をとる
                var material = x.GetComponent<Renderer>().material;

                // 半透明にする
                if(x.GetComponent<Renderer>().material.GetTag("RenderType", true) != "Transparent")
                {
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;    
                }

                // アルファのみ変更
                x.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);
            });

        // 透明度を戻す
        this.OnTriggerExitAsObservable()
            .Where(x => LayerMask.LayerToName(x.gameObject.layer) != "Ignore Alpha")
            .Where(x => x.GetComponent<Renderer>() != null)
            .Subscribe(x =>
            {
                Debug.Log("exit");
                // 色をとる
                var color = x.GetComponent<Renderer>().material.color;
                // アルファのみ変更(完全に不透明にする)
                x.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);
            });

        // 正しい参照に直す
        var goal = GetComponent<GoalDetector>();
        // 全部のオブジェクトの色を戻す
        this.ObserveEveryValueChanged(x => goal)
            .DistinctUntilChanged()
            .Where(x => x == true)
            .Subscribe(_ =>
            {
                foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.activeInHierarchy && obj.GetComponent<Renderer>() && LayerMask.LayerToName(obj.gameObject.layer) != "Ignore Alpha")
                    {
                        // 色をとる
                        var color = obj.GetComponent<Renderer>().material.color;
                        // アルファのみ変更(完全に不透明にする)
                        obj.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);
                    }
                }
            });
    }

    // Update is called once per frame
    void Update () {

    }
}
