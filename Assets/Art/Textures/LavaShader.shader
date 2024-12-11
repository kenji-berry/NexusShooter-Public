Shader "Custom/LavaShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _FlowSpeed ("Flow Speed", Float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        float _FlowSpeed;

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float time = _Time.y * _FlowSpeed; // Use time to animate the effect
            float2 flowOffset = sin(time + IN.uv_MainTex * 10.0) * 0.05;
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex + flowOffset).rgb;
        }
        ENDCG
    }
    Fallback "Diffuse"
}
