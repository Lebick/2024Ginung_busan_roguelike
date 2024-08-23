Shader "CustomShader/FadeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ImageTex ("ImageTexture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Progress ("Progress", Range(0,1)) = 0
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
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
            sampler2D _ImageTex;
            float4 _Color;
            float _Progress;
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_ImageTex, i.uv);

                if(col.r < _Progress)
                    return _Color;

                return float4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
