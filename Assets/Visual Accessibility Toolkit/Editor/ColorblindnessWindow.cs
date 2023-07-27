using UnityEngine;
using UnityEditor;

/// <summary>
/// //A script to set colorblindness settings from unity editor
/// </summary>
public class ColorblindnessWindow : EditorWindow
{
    private ColorblindnessMode _colorblindnessMode;

    [MenuItem("Visual Accessibility Toolkit/Colorblindness Simulation Window")]
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

        // Apply the colorblindness shader to the main camera
    }

    private void ApplyColorblindnessShader()
    {
        if (Camera.main != null)
        {
            Camera.main.SetReplacementShader(null, null);

            if (Camera.main.GetComponent<ColorblindnessSimulation>() != null)
                Camera.main.GetComponent<ColorblindnessSimulation>().SetSimulationConfiguration(_colorblindnessMode);
            else
                MonoBehaviour.print("\'ColorblindnessSimulation\' script is not found on MainCamera.");
        }
        else
            MonoBehaviour.print("No Camera with tag \'MainCamera\' is found in this scene.");


    }
}