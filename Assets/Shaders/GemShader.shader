Shader "Custom/GemShader_AdvancedGlass"
{
    Properties
    {
        _Color("Base Color", Color) = (0.28, 0.45, 0.70, 0.6)
        _MainTex("Base Texture", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 1.0
        _Metallic("Metallic", Range(0,1)) = 0.0
        _EmissionColor("Emission", Color) = (0.28, 0.45, 0.70, 1)
        _FresnelPower("Fresnel Power", Range(0.1, 10)) = 2.5
        _TransmissionWeight("Transmission Weight", Range(0, 2)) = 1.0
        _RefractionStrength("Fake Refraction Strength", Range(0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        Lighting On

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _EmissionColor;
        float _FresnelPower;
        float _TransmissionWeight;
        float _RefractionStrength;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldRefl;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 refractionUV = IN.uv_MainTex + (normalize(IN.viewDir).xy * _RefractionStrength);
            fixed4 baseCol = tex2D(_MainTex, refractionUV) * _Color;

            float fresnel = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _FresnelPower);
            float transmission = saturate(_TransmissionWeight * fresnel);

            o.Albedo = baseCol.rgb * (1.0 - transmission) + _EmissionColor.rgb * transmission;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = _EmissionColor.rgb * transmission;
            o.Alpha = baseCol.a;
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
}
