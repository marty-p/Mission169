Shader "Unlit/Grayscale Effect"
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
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);

				if (texcol.r < 0.86 &&  texcol.r >= 0.67) {
					texcol.rgb = float3(1, 0.9, 0.37);
				}
				else if ( texcol.r >= 0.65 ) {
					texcol.rgb = float3(1, 0.9, 0.56);
				}
				else if (texcol.r >= 0.47) {
					texcol.rgb = float3(1, 0.7, 0.16);
				}
				else if (texcol.r >= 0.31) {
					texcol.rgb = float3(0.87, 0.43, 0);
				}
				else if (texcol.r >= 0.188) {
					texcol.rgb = float3(0.71, 0.28, 0);
				}
				else {
					texcol.rgb = float3(0.56, 0.16, 0);
				}
				
				texcol.rgb *= texcol.a;
				return texcol;
			}
			ENDCG

		}
	}
}
