Shader "Unlit/emmi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmMap ("Emissive Map", 2D) = "white" {}
        _Brightness ("Brightness value", range(0,2)) =0
        [HDR] _Color1("color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "RenderType"="Opaque" 
        "Queue"="Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _MainTex, _EmMap;
            float4 _MainTex_ST, _Color1;
            float _Brightness;

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
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 em = tex2D(_EmMap, i.uv);
                fixed4 glow = (em.r * _Color1)*_Brightness;
                return col + glow;
            }
            ENDCG
        }
    }
}
