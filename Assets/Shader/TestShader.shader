/*「グループ名 / シェーダー名」は、初期設定で「Custom / ファイルの名前」になっていますが、
自由に変更する事ができます。グループ名は / で区切って複数記述すると階層を付けられます。
解り易い整理された名前にしましょう。*/

Shader "Custom/TestShader" 
{
	Properties 
	{
		/*プロパティ変数名 ("インスペクター表示名", 変数の型) = 初期値*/
		_Color		("Color",		 Color)		 = (1,1,1,1)
		_MainTex	("Albedo (RGB)", 2D)		 = "white" {}
		_Glossiness ("Smoothness",	 Range(0,1)) = 0.5
		_Metallic	("Metallic",	 Range(0,1)) = 0.0
	}

/*ここが一番重要です。宣言したプロパティ達をどのように利用して描画するのかを
実際に記述する箇所になります。*/
	SubShader
		{

		Tags { "RenderType"="Opaque" }

		LOD 200

			
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard fullforwardshadows

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c	 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo	 = c.rgb;	
			o.Metallic	 = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha		 = c.a;

		}
		ENDCG
	}

	/*すべりどめシェーダー名*/
	FallBack "Diffuse"
}
