using UnityEngine;
using UnityEditor;

/// <summary>
/// //A script to set colorblindness settings from unity editor
/// </summary>
public class ColorblindnessWindow : EditorWindow
{
    public enum ColorblindnessMode
    {
        Normal,
        Protanopia,
        ProtanopiaFix,
        Deuteranopia,
        DeuteranopiaFix,
        Tritanopia
    }
    private float _intensity = 1f;

    private ColorblindnessMode _colorblindnessMode;

    [MenuItem("VisualToolkit/Simulate Colorblindness")]
    public static void ShowWindow()
    {
        GetWindow<ColorblindnessWindow>("Colorblindness Simulator");
    }

    void OnGUI()
    {
        GUILayout.Label("Colorblindness Simulator", EditorStyles.boldLabel);

        // Add buttons to switch between different colorblindness modes
        if (GUILayout.Button("Normal"))
        {
            _colorblindnessMode = ColorblindnessMode.Normal;
            Camera.main.GetComponent<CameraSimulationEffect>().colorBlindMode = false;
            ApplyColorblindnessShader();
        }
        if (GUILayout.Button("Protanopia"))
        {
            _colorblindnessMode = ColorblindnessMode.Protanopia;
            ApplyColorblindnessShader();
        }

        if (GUILayout.Button("Deuteranopia"))
        {
            _colorblindnessMode = ColorblindnessMode.Deuteranopia;
            ApplyColorblindnessShader();

        }

        if (GUILayout.Button("Tritanopia"))
        {
            _colorblindnessMode = ColorblindnessMode.Tritanopia;
            ApplyColorblindnessShader();

        }

        // GUILayout.Label("Intensity");
        //    _intensity = GUILayout.HorizontalSlider(_intensity, 0f, 1f);


        // Apply the colorblindness shader to the main camera
    }

    private void ApplyColorblindnessShader()
    {
        Camera.main.SetReplacementShader(null, null);

        switch (_colorblindnessMode)
        {
            case ColorblindnessMode.Normal:
                MonoBehaviour.print("Normal");
                Camera.main.GetComponent<CameraSimulationEffect>().simulationMaterial.SetInt("_Type", 0);
                Camera.main.GetComponent<CameraSimulationEffect>().colorBlindMode = false;
                Shader.SetGlobalInteger("_Type", 0);
                break;
            case ColorblindnessMode.Protanopia:
                Camera.main.GetComponent<CameraSimulationEffect>().simulationMaterial.SetInt("_Type", 1);
                Camera.main.GetComponent<CameraSimulationEffect>().colorBlindMode = true;
                Shader.SetGlobalInteger("_Type", 1);
                MonoBehaviour.print("Protonopia");
                break;
            case ColorblindnessMode.Deuteranopia:
                Camera.main.GetComponent<CameraSimulationEffect>().simulationMaterial.SetInt("_Type", 2);
                Camera.main.GetComponent<CameraSimulationEffect>().colorBlindMode = true;
                Shader.SetGlobalInteger("_Type", 2);
                MonoBehaviour.print("Deuteranopia");
                break;
            case ColorblindnessMode.Tritanopia:
                Camera.main.GetComponent<CameraSimulationEffect>().simulationMaterial.SetInt("_Type", 3);
                Camera.main.GetComponent<CameraSimulationEffect>().colorBlindMode = true;
                Shader.SetGlobalInteger("_Type", 3);
                MonoBehaviour.print("Tritanopia");
                break;
            default:
                break;

                // Shader.SetGlobalInteger("_CorrectionType", (int)colorCorrectionType);
        }
    }
}