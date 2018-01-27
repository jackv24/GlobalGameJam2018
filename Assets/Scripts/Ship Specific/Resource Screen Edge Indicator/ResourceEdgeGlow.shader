Shader "Hidden/ResourceEdgeGlow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EffectAlpha("Effect Alpha", Float) = 0
		_PositionCount("Position Count", Int) = 0
		_TexWidth("Texture Width", Int) = 0
		_TexHeight("Texture Height", Int) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Positions[64];
			float _EffectAlpha;
			int _PositionCount;
			int _TexWidth;
			int _TexHeight;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				col *= float4(i.uv.x, i.uv.y, 0, 1);

				return col;
			}
			ENDCG
		}
	}
}
