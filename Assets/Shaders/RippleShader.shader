Shader "Unlit/RippleShader"
{
    Properties 
    {
        _Color1 ("Color 1", Color) = (1,1,1,0.1)
        _Color2 ("Color 2", Color) = (1,1,1,0.5)
        _Tiling ("Tiling", Range(1, 500)) = 10
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
			
			fixed4 _Color1;
            fixed4 _Color2;
			int _Tiling;
			float _Direction;
			float _Speed;

            float2 rotatePoint(float2 pt, float2 center, float angle) 
            {
                float sinAngle = sin(angle);
                float cosAngle = cos(angle);
                pt -= center;
                float2 r;
                r.x = pt.x * cosAngle - pt.y * sinAngle;
                r.y = pt.x * sinAngle + pt.y * cosAngle;
                r += center;
                return r;
            }

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
            {
                float pos = lerp(i.uv.x, i.uv.y, _Direction) * _Tiling + (_Time.y * _Speed);
                fixed value = floor(frac(pos) + 0.5) ;
                return lerp(_Color1, _Color2, value);
            }
			ENDCG
		}
	}
}
