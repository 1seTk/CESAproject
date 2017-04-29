/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeImage : UnityEngine.UI.Graphic , IFade
{
    //フェードイン用のテクスチャ
    //[SerializeField]
    private Texture maskTexture = null;

    [SerializeField, Range (0, 1)]
	private float cutoutRange;

	public float Range
    {
		get
        {
			return cutoutRange;
		}
		set
        {
			cutoutRange = value;
			UpdateMaskCutout (cutoutRange);
		}
	}

	protected override void Start ()
	{
		base.Start ();
		//UpdateMaskTexture (maskTexture);
	}


	private void UpdateMaskCutout (float range)
	{
		enabled = true;

        material.SetFloat ("_Range", 1 - range);

		if (range <= 0.0f)
        {
            range = 0.0f;

            //使用不可にする
			//this.enabled = false;
		}
	}


	public void UpdateMaskTexture (Texture texture)
	{
        if (texture == null)
        {
            return;
        }

        //マテリアルにテクスチャを設定
		material.SetTexture ("_MaskTex", texture);

        //マテリアルに色を設定
		material.SetColor ("_Color", color);
	}


	#if UNITY_EDITOR
	protected override void OnValidate ()
	{
		base.OnValidate ();
		UpdateMaskCutout (Range);
		UpdateMaskTexture (maskTexture);
	}
	#endif
}
