Shader "Unlit/aaa"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Intensity("Intensity", Range(0,1)) = 0.5
        
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "RenderType"="Transparent"
    "Queue"="Transparent"
    }
        LOD 100
        GrabPass{
            "_BackgroundTex"
}
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

            sampler2D _MainTex, _BackgroundTex, _NoiseTex;
            float4 _MainTex_ST, _BackgroundTex_ST;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 d = tex2D(_NoiseTex, i.uv);
                float4 p = i.uv + (d * _Intensity);
                fixed4 col = tex2Dproj(_BackgroundTex, p);
                fixed4 alpha = tex2D(_MainTex, i.uv);
                alpha.rgb = col.rgb;

                return alpha;
            }
            ENDCG
        }
    }
}
