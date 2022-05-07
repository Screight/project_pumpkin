Shader "Unlit/shakingGShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Displace ("Displacement Texture", 2D) = "white" {}
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
            float4 _strenght;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half4 d = tex2D(_Displace, i.uv);
                
                fixed4 col = tex2D(_MainTex, d);
                return col;
            }
            ENDCG
        }
    }
}
