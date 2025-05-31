Shader "Custom/SimpleWaterTransparent"
{
    Properties
    {
        _Color ("Water Color", Color) = (0.2, 0.5, 0.7, 0.5) // alpha deðeri düþük
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpSpeed ("Bump Scroll Speed", Vector) = (0.01, 0.005, 0, 0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.8
        _Metallic ("Metallic", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off // Þeffaflýk için derinlik yazma kapalý olmalý

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _BumpMap;
        float4 _BumpSpeed;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;

        struct Input
        {
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 bumpUV = IN.uv_BumpMap + _Time.y * _BumpSpeed.xy;
            o.Albedo = _Color.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, bumpUV));
            o.Smoothness = _Glossiness;
            o.Metallic = _Metallic;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
