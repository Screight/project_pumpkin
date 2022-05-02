Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Intensity("Wave Speed", Range(0,1)) = 0.5
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _NoiseTex("Extra Wave Noise", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
   
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        GrabPass
        {
           "_BackgroundTexture"
        }

        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct v2f
        {
            float4 grabPos : TEXCOORD0;
            float4 pos : SV_POSITION;
        };


        float _Intensity;

        struct Input
        {
            float2 _BackgroundTexture;
        };
        sampler2D _BackgroundTexture, _NoiseTex;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        
        half4 frag(v2f i) : SV_Target
        {
        half4 d = tex2D(_NoiseTex, i.grabPos);
        float4 p = i.grabPos +(d* _Intensity);
        half4 bgcolor = tex2Dproj(_BackgroundTexture, p);
        return bgcolor;
        }
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_BackgroundTexture, IN._BackgroundTexture) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }

    
    FallBack "Diffuse"
}
