Shader "GrabPassInvert"
{
    Properties
    {
        
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Color" , Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
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
                    float2 uv : TEXCOORD0;
                };
                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };
                sampler2D _MainTex;
                v2f vert(appdata v)
                {
                    v2f o;
                    o.uv = v.uv;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    return o;
                }
                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 texColor = tex2D(_MainTex, i.uv);
                    texColor.a = 0 ;
                    return texColor;
                }
                ENDCG
            }
        }
}