Shader "Custom/Particles/URP_HDRBloom_Additive"
{
    Properties
    {
        [MainTexture] _MainTex ("Particle Texture", 2D) = "white" {}

        // HDR color picker (lets you push beyond 1.0)
        [HDR] _Tint ("Tint (HDR)", Color) = (1,1,1,1)

        // Extra multiplier for bloom strength
        _Intensity ("HDR Intensity", Range(0, 50)) = 5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        // Common render state for particles
        Cull Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha One // ADDITIVE

        // -------- PASS for URP Forward Renderer --------
        Pass
        {
            Name "URPForward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Tint;
                float  _Intensity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;      // Particle vertex color
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // HDR output for bloom: can exceed 1.0
                half3 rgb = tex.rgb * IN.color.rgb * _Tint.rgb * _Intensity;
                half  a   = tex.a   * IN.color.a   * _Tint.a;

                return half4(rgb, a);
            }
            ENDHLSL
        }

        // -------- PASS for URP 2D Renderer --------
        Pass
        {
            Name "URP2D"
            Tags { "LightMode"="Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Tint;
                float  _Intensity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half3 rgb = tex.rgb * IN.color.rgb * _Tint.rgb * _Intensity;
                half  a   = tex.a   * IN.color.a   * _Tint.a;
                return half4(rgb, a);
            }
            ENDHLSL
        }
    }
}