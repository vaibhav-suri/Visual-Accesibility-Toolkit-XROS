Shader "Custom/ColorCorrector"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _CorrectionType("Colorblind Mode", Range(0, 3)) = 0
        _Intensity("Intensity", Range(0, 1)) = 0.5
    }

        CGINCLUDE
#include "UnityCG.cginc"

            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
        uniform half4 _MainTex_ST;
        uniform float _Intensity;
        uniform float _CorrectionType;

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

        float3 protanopeValues = float3(0.0f, 2.02344f, -2.52581f);
         float3 deuteranopeValues = float3(1.0f, 0.494207f, 1.24827f);
        float3 tritanopeValues = float3(1.0f, 0.0f, 0.801109f);
        // color-shifting matrices
        static float3x3 color_matrices[4] = {
            // normal vision - identity matrix
            float3x3(
              1.0f, 0.0f, 0.0f,
              0.0f, 1.0f, 0.0f,
              0.0f, 0.0f, 1.0f
            ),
            float3x3(
            0.0f, 2.02344f, -2.52581f,
            0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f
            ),

            float3x3(
            1.0f, 0.0f, 0.0f,
            0.494207f, 0.0f, 1.24827f,
            0.0f, 0.0f, 1.0f
            ),

        float3x3(
            1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f,
            -0.395913f, 0.801109f, 0.0f
)
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
            float3x3 colorBlindMatrix = color_matrices[_CorrectionType];
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
            float3 L = (17.8824f * color.r) + (43.5161f * color.g) + (4.11935f * color.b);
            float3 M = (3.45565f * color.r) + (27.1554f * color.g) + (3.86714f * color.b);
            float3 S = (0.0299566f * color.r) + (0.184309f * color.g) + (1.46709f * color.b);
       
		    float l = interpolatedMatrix[0][0] * L + interpolatedMatrix[0][1] * M + interpolatedMatrix[0][2] * S;
		    float m = interpolatedMatrix[1][0] * L + interpolatedMatrix[1][1] * M + interpolatedMatrix[1][2] * S;
		    float s = interpolatedMatrix[2][0] * L + interpolatedMatrix[2][1] * M + interpolatedMatrix[2][2] * S;
	     
            float4 error;
            error.r = (0.0809444479f * l) + (-0.130504409f * m) + (0.116721066f * s);
            error.g = (-0.0102485335f * l) + (0.0540193266f * m) + (-0.113614708f * s);
            error.b = (-0.000365296938f * l) + (-0.00412161469f * m) + (0.693511405f * s);
            error.a = 1;

            return error.rgba;
        //    float3 x = mul(color.rgb, (interpolatedMatrix));
         //   return fixed4(x, 1.0);
            
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