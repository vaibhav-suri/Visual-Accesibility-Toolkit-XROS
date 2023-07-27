Shader "Custom/Colorblind"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Type("Colorblind Mode", Range(0, 3)) = 1
        _Intensity("Intensity", Range(0, 1)) = 1
        [Toggle] _IsNoEditor("Is not in Editor", Integer) = 0
        [HideInInspector]_Color("Color", Color) = (1,1,1,1)
    }
        SubShader
        {
            Cull Off ZWrite Off ZTest Always

            Pass
            {
                CGPROGRAM
                #pragma target 3.0

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                int _Type;
                float _Intensity;
                bool _IsNoEditor;
                float4 _Color;
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                // color-shifting matrices
                static float3x3 color_matrices[4] = {
                    // normal vision - identity matrix
                    float3x3(
                        1.0f, 0.0f, 0.0f,
                        0.0f, 1.0f, 0.0f,
                        0.0f, 0.0f, 1.0f),
                    // Protanopia - blindness to long wavelengths
                    float3x3(
                        0.567f, 0.433f, 0.0f,
                        0.558f, 0.442f, 0.0f,
                        0.0f, 0.242f, 0.758f),
                    // Deuteranopia - blindness to medium wavelengths
                    float3x3(
                        0.625f, 0.375f, 0.0f,
                        0.7f, 0.3f, 0.0f,
                        0.0f, 0.3f, 0.7f),
                    // Tritanopia - blindness to short wavelengths
                    float3x3(
                        0.95f, 0.05f, 0.0f,
                        0.0f, 0.433f, 0.567f,
                        0.0f, 0.475f, 0.525f)
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                // vertex shader
                v2f vert(appdata_img v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
                    return o;
                }

                // fragment shader
                half4 frag(v2f i) : SV_Target
                {
                    // read the color from input texture
                    half4 color = _Color;
                    if (_IsNoEditor) color = tex2D(_MainTex, i.uv);
                    // matrix multiplication with color-shifting matrix - index specified by '_Type' variable
                    float3 x = mul(color.rgb, color_matrices[_Type]);
                    return half4(x, 1.0f);
                    // return _Color;
                }

                ENDCG
            }
        }
}