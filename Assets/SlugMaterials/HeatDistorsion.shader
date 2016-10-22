Shader "Custom/HeatDistortion"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeatMap("Heat map", 2D) = "bump" {}
		_Intensity("Intensity", Range(0,1)) = 0.03
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
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
			sampler2D _HeatMap;
			float  _Intensity;

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
				float2 heatMapCoord = i.uv;
				heatMapCoord.y -= _Time / 2;
				heatMapCoord.y = frac(heatMapCoord.y);

				float4 heatMapVal = tex2D(_HeatMap, heatMapCoord);

				float2 heatPosOffset = heatMapVal.rg;
				heatPosOffset -= float2(0.5, 0.5);
				heatPosOffset *= 2;

				//Controlling severity
				heatPosOffset *= _Intensity;

				// attenuate with height of the texture
				float attenuation = clamp( (1 - i.uv.y), 0.3, 1);
				heatPosOffset *= attenuation;

				float2 heatTexCoord = i.uv + heatPosOffset;
				//float2 heatTexCoord = i.uv;

				fixed4 texCol = tex2D(_MainTex, heatTexCoord);
				texCol.rgb *= texCol.a;
				return texCol;
			}
		ENDCG
		}
	}
}