Shader "Custom/ColorShift2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorShiftX ("Color Shift X", Range(-1, 1)) = 0.1
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent"}
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ColorShiftX;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.r = tex2D(_MainTex, i.uv + float2(_ColorShiftX, 0)).r;
                col.b = tex2D(_MainTex, i.uv - float2(_ColorShiftX, 0)).b;
                return col;
            }
            ENDCG
        }
    }
}