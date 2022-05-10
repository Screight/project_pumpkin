// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CameraFX"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("CutOff color", Color) = (1,1,1,1)
        _Cutoff ("Cutoff value", Range(0,1)) = 0.5
        _Displacement ("Displacement Texture", 2D)=  "white"{}
    }
    SubShader
    {
        
        Tags { 
        "Queue"="Transparent"
        }

        Pass
        {
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex, _Displacement;
            float4 _MainTex_ST, _Color;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
               fixed4 transit = tex2D(_Displacement, i.uv);
            fixed2 dir = normalize(float2((transit.r - 0.5) * 2, (transit.g - 0.5) * 2));
            fixed4 col = tex2D(_MainTex, i.uv + _Cutoff * dir);
                if (transit.b < _Cutoff)
                return _Color;

            return col;
            }
            ENDCG
        }
    }
}
