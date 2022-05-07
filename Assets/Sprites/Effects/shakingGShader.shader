Shader "Unlit/shakingGShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Displace ("Displacement Texture", 2D) = "bump" {}
        _strenght("Intensity", Range(0, 1)) = 0.5
        
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
            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _Displace;
            float4 _Displace_ST, _MainTex_ST;
            float _strenght;
            


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                _MainTex_ST.x +=  unity_DeltaTime;
                half4 d = tex2D(_Displace, _MainTex_ST);
                float4 p = i.uv + (d * _strenght);
                fixed4 col = tex2D(_MainTex, p);
                return col;
            }
            ENDCG
        }
    }
}
