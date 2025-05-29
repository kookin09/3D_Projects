Shader "Unlit/leejucustomshader3Color"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (0.38, 0.2, 0.1, 1.0)     // 진한 갈색
        _ColorMiddle ("Middle Color", Color) = (0.6, 1.0, 0.9, 1.0) // 민트
        _ColorBottom ("Bottom Color", Color) = (0.38, 0.2, 0.1, 1.0)// 진한 갈색
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

            fixed4 _ColorTop;
            fixed4 _ColorMiddle;
            fixed4 _ColorBottom;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.y < 0.33)
                    return _ColorBottom;
                else if (i.uv.y < 0.66)
                    return _ColorMiddle;
                else
                    return _ColorTop;
            }
            ENDCG
        }
    }
}
