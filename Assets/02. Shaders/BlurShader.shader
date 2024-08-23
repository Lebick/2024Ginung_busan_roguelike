Shader "CustomShader/newTEst"
{
    Properties
    {
        _Radius("Radius", Range(1, 100)) = 1 //
        _DownSample("DownSample", Range(1, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Overlay" "RenderType"="Transparent"} //
        GrabPass{}

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

            sampler2D _GrabTexture;         //반드시 이 변수명으로 사용해야함.
            float4 _GrabTexture_TexelSize;  //반드시 이 변수명으로 사용해야함.
            float _Radius;
            float _DownSample;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = o.uv = o.vertex.xy * 0.5 + 0.5;
                o.uv.y = 1.0 - o.uv.y;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = float4(0, 0, 0, 0);
                int samples = 0;
                float2 offset = _GrabTexture_TexelSize.xy * _DownSample;

                for(int x = -_Radius; x <= _Radius; x++)
                {
                    for(int y = -_Radius; y <= _Radius; y++)
                    {
                        color += tex2D(_GrabTexture, i.uv + float2(x,y) * offset);
                        samples++;
                    }
                }

                return color / samples;
            }
            ENDCG
        }
    }
}
