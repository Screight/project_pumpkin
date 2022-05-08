Shader "Unlit/shakingGShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Displace ("Displacement Texture", 2D) = "bump" {}
        _strenght("Intensity", Range(0, 1)) = 0.5

        _ScrollSpeeds("Scroll Speeds", vector) = (-5, 0, 0, 0)
        
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
            float4 _Displace_ST, _MainTex_ST, _ScrollSpeeds;
            float _strenght;
            
            
            float random(float2 p) {
                return cos(dot(p, float2(23.14069263277926, 2.665144142690225))) * 12345.6789; 
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                float r = sin(_Time.y * 15);
                float z = sin(_Time.y * 5);

                o.uv += vector(_ScrollSpeeds.x * r/100, _ScrollSpeeds.y * z/200, 0,0) ;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                half4 d = tex2D(_Displace, _MainTex_ST);
                float4 p = i.uv + (d * _strenght);
                fixed4 col = tex2D(_MainTex, p);
                return col;
            }
            ENDCG
        }
    }
}
