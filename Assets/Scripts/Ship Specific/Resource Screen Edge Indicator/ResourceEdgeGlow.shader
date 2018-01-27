Shader "Hidden/ResourceEdgeGlow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
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
			//float4[] _Positions;
			//float _EffectAlpha;
			//int _PositionCount;
			//int _TexWidth;
			//int _TexHeight;

			fixed4 frag (v2f i) : SV_Target
			{
				//float4 screenSpacePos = ComputeScreenPos(i.vertex);
				return fixed4(1, 1, 1, 1);

				float x = screenSpacePos.x / _TexWidth;
				float y = screenSpacePos.y / _TexHeight;
				
				
				
				
				
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
