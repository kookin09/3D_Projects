Shader "Unlit/leejucustomshader"
{
    Properties
    {
        _Color1 ("Top Color", Color) = (0.4706, 0.7059, 0.3922, 1.0) // Sage Green
        _Color2 ("Bottom Color", Color) = (0.3922, 0.8627, 0.5490, 1.0) // Spring Green
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 수직 그라디언트
                return lerp(_Color2, _Color1, i.uv.y);
            }
            ENDCG
        }
    }
}
