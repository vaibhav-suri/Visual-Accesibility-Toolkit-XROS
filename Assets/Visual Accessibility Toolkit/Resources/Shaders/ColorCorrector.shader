Shader "Custom/ColorCorrector"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
            _Type("Colorblind Mode", Range(0, 3)) = 0
        _Intensity("Intensity", Range(0, 1)) = 0.5
    }

        CGINCLUDE
#include "UnityCG.cginc"

            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
        uniform half4 _MainTex_ST;
        uniform float _Intensity;
        uniform float _Type;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
        };
        // color-shifting matrices
        static float3x3 color_matrices[4] = {
            // normal vision - identity matrix
            float3x3(
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f),

            // Protanopia - blindness to long wavelengths (correction matrix)
            float3x3(
                0.817f, 0.183f, 0.0f,
                0.333f, 0.667f, 0.0f,
                0.0f, 0.125f, 0.875f),

            // Deuteranopia - blindness to medium wavelengths (correction matrix)
            float3x3(
                0.8f, 0.2f, 0.0f,
                0.258f, 0.742f, 0.0f,
                0.0f, 0.142f, 0.858f),

            // Tritanopia - blindness to short wavelengths (correction matrix)
            float3x3(
                0.95f, 0.05f, 0.0f,
                0.0f, 0.433f, 0.567f,
                0.0f, 0.475f, 0.525f)
        };
        v2f vertPass1(appdata v)
        {
            v2f o;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o)
                o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

        fixed4 fragPass1(v2f i) : SV_Target
        {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
            fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
            col.rgb =   col.rgb; // Invert colors
            
            return col;
        }

            v2f vertPass2(appdata v)
        {
            v2f o;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o)
                o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }
        // Initialize an interpolated matrix

        
        fixed4 fragPass2(v2f i) : SV_Target
        {
             fixed4 color = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
        float3x3 interpolatedMatrix;
        float3x3 normalMatrix = color_matrices[0];
        float3x3 colorBlindMatrix = color_matrices[_Type];
        // Interpolate each element of the matrix based on _Intensity
        interpolatedMatrix[0][0] = lerp(normalMatrix[0][0], (colorBlindMatrix)[0][0], _Intensity);
        interpolatedMatrix[0][1] = lerp(normalMatrix[0][1], (colorBlindMatrix)[0][1], _Intensity);
        interpolatedMatrix[0][2] = lerp(normalMatrix[0][2], (colorBlindMatrix)[0][2], _Intensity);
        interpolatedMatrix[1][0] = lerp(normalMatrix[1][0], (colorBlindMatrix)[1][0], _Intensity);
        interpolatedMatrix[1][1] = lerp(normalMatrix[1][1], (colorBlindMatrix)[1][1], _Intensity);
        interpolatedMatrix[1][2] = lerp(normalMatrix[1][2], (colorBlindMatrix)[1][2], _Intensity);
        interpolatedMatrix[2][0] = lerp(normalMatrix[2][0], (colorBlindMatrix)[2][0], _Intensity);
        interpolatedMatrix[2][1] = lerp(normalMatrix[2][1], (colorBlindMatrix)[2][1], _Intensity);
        interpolatedMatrix[2][2] = lerp(normalMatrix[2][2], (colorBlindMatrix)[2][2], _Intensity);
            float3 x = mul(color.rgb, (interpolatedMatrix));
            return fixed4(x, 1.0);
            
        }

            fixed4 frag(v2f i) : SV_Target
        {
             fixed4 color = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
            float3 x = mul(color.rgb, color_matrices[0]);
            return fixed4(x, 1.0);
        }

            ENDCG

            SubShader
        {
            Cull Off ZWrite Off ZTest Always

                Pass
            {
                CGPROGRAM
                #pragma vertex vertPass1
                #pragma fragment frag 
                ENDCG
            }

               

                Pass
            {
                CGPROGRAM
                #pragma vertex vertPass1
                #pragma fragment fragPass2
                ENDCG
            }
        }
}