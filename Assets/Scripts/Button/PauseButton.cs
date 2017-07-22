// ---------------------------------------
// Brief : 
// 
// Date  : 
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PauseButton : MonoBehaviour
{
	void Start ()
	{
		transform.DOMove(new Vector3(0, 0, 0), 0.2f);
	}
}
