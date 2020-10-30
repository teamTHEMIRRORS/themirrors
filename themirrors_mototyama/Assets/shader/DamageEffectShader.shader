Shader "Custom/DamageEffectShader"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _effectPosition("_effectPosition",Vector) = (0.5,0.5,0,0)
        _effectVolume("_effectVolume",Range(0,1.0)) = 0.0
       [Toggle] _isReverseUV("_isReverseUV",Int) = 0
    }
    SubShader
    {
        // No culling or depth
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
            int _isReverseUV;
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraColorTexture;
            float4 _MainTex_TexelSize;
            float4 _effectPosition;
            float _effectVolume;

            fixed4 frag(v2f i) : SV_Target
            {
                # if UNITY_UV_STARTS_AT_TOP
                    i.uv.y = 1 - i.uv.y;
                # endif
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 effectVec = _effectPosition.xy - i.uv;
                if (_effectVolume)
                {
                    for (float j = 1; j < 5; j++)
                    {
                        col += tex2D(_MainTex, i.uv + _effectVolume * j / 5 * (effectVec)) * float4(
                            _effectVolume, -_effectVolume, -_effectVolume, 0);
                    }
                }
                return col;
            }
            ENDCG
        }
        //UsePass "Hidden/Universal Render Pipeline/UberPost/UberPost"
    }
}
