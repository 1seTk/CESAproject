using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    [SerializeField]
    public float pos_x = 4.65f;
    public float pos_y = 5.47f;
    public float pos_z;

    GameObject Player;
    GameObject mainCamera;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {

        mainCamera.transform.position = new Vector3(pos_x, pos_y, Player.transform.position.z - 10);

    }
}