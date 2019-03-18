// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BlueScale"
{
	Properties{
		_MainTex("Texture", 2D) = "white" { }
	}
	SubShader{

			Tags
		{
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
			
			
			Pass{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
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

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);

				if (texcol.r < 0.12) {
					texcol.rgb = float3(0.28, 0.5, 0.63);
				}
				else if ( texcol.r >= 0.12 && texcol.r < 0.23) {
					texcol.rgb = float3(0.44, 0.66, 0.78);
				}
				else if (texcol.r >= 0.23 && texcol.r < 0.39) {
					texcol.rgb = float3(0.6, 0.78, 0.88);
				}
				else if (texcol.r >= 0.39 &&  texcol.r < 0.7) {
					texcol.rgb = float3(0.75, 0.91, 0.97);
				}
				else if (texcol.r >= 0.7 && texcol.r < 0.86) {
					texcol.rgb = float3(0.88, 0.97, 0.97);
				}
				else {
					texcol.rgb = float3(0.97, 0.97, 0.91);
				}
				
				texcol.rgb *= texcol.a;
				return texcol;
			}
			ENDCG

		}
	}
}
