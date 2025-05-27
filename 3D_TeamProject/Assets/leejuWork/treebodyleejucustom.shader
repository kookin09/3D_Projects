Shader "Unlit/treebodyleejucustomshader"
{
    Properties
    {
        _Color1 ("Left Side Color", Color) = (0.545, 0.353, 0.168, 1.0) // 밝은 갈색 (왼쪽)
        _Color2 ("Right Side Color", Color) = (0.231, 0.184, 0.184, 1.0) // 어두운 갈색 (오른쪽)
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
