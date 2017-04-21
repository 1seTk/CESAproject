using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float speed = 0.01f;
    private bool canMove = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Spaceキーを押した");

                transform.position += new Vector3(0.0f, 0.0f, speed) * Time.deltaTime;
            }
            //transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        // 自分以外と衝突していたら
        if(hit.transform.GetInstanceID() != transform.GetInstanceID())
        {
            Debug.Log("000");
            // 操作不能にする
            canMove = false;

            // 子オブジェクトのパーティクルを参照
            var par = GetComponentInChildren<ParticleSystem>();

            // パーティクルを再生
            par.Play();

            // プレイヤーの描画をしないようにする    
            transform.GetComponent<Renderer>().enabled = false;

            // プレイヤーの当たり判定を取らないようにする
            var col = transform.GetComponents<Collider>();

            foreach (var item in col)
            {
                item.enabled = false;
            }
        }
    }
}