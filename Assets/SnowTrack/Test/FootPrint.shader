Shader "Unlit/FootPrint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            half _FootPrintSize;
            half _WorldSize;
            half _Atten;
            float3 _WorldPosition;
            float3 _LastWorldPosition;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 deltaPos = _LastWorldPosition - _WorldPosition;
                float2 deltaUV = float2(deltaPos.x, deltaPos.z) / _WorldSize;
                float2 lastUV = i.uv - deltaUV;
                half4 col = tex2D(_MainTex, lastUV);
                half2 lastFootPrints = step(0, lastUV) * step(lastUV, 1);
                half lastFootPrint = col.r * lastFootPrints.x * lastFootPrints.y;
                lastFootPrint = saturate(lastFootPrint - _Atten);

                float2 worldPos = float2(_WorldPosition.x, _WorldPosition.z) + (i.uv - 0.5) * _WorldSize;
                float2 difPos = abs(worldPos - float2(_WorldPosition.x, _WorldPosition.z));
                half2 footPrints = step(difPos, _FootPrintSize / 2);
                half footPrint = footPrints.x * footPrints.y;

                footPrint = saturate(footPrint + lastFootPrint);

                return half4(footPrint, 0, 0, 1);
            }
            ENDCG
        }
    }
}
