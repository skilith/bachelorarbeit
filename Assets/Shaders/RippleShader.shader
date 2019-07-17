// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RippleShader"
{
    Properties 
    {
        _Color1 ("Color 1", Color) = (1,1,1,0.1)
        _Color2 ("Color 2", Color) = (1,1,1,0.5)
        _Tiling ("Tiling", Range(1, 500)) = 128
        _Direction ("Direction", Range(0, 1)) = 0
        _Speed ("Speed", Float) = 1
    }
    
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
				float4 screenPos:TEXCOORD2;
			};
			
			fixed4 _Color1;
            fixed4 _Color2;
			int _Tiling;
			float _Direction;
			float _Speed;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
            {
                fixed4 transColor = (1, 1, 1, 0);
             
                //float pos = lerp(i.uv.x, i.uv.y, _Direction) * _Tiling + (_Time.y * _Speed);
                //fixed value = floor(frac(pos) + 0.5) ;
                     
                float pos = cos(distance(i.uv.xy, float2(0.5,0.5)) * _Tiling + _Time.y * _Speed);
                fixed value = floor(pos + 1) ;

                i.screenPos.xy /= i.screenPos.w;
                    
                if(i.screenPos.x < 0.66 && i.screenPos.x > 0.33 && i.screenPos.y < 0.95  && i.screenPos.y > 0.65) { 
                    return lerp(_Color1, _Color2, value);
                }
                
                return transColor;
            }
			ENDCG
		}
	}
}
