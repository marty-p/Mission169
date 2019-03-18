// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/brightnessControl"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BrightnessAmount("Brightness Amount", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite On
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha
			
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float _BrightnessAmount;
			
			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.texcoord;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = col.rgb + _BrightnessAmount;

				//col.rgb = float3(0.56, 0.16, 0);
				col.rgb *= col.a;
				return col;
			}
			ENDCG
		}
	}
}
