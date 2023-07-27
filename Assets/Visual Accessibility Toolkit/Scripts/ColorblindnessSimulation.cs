using UnityEngine;
using UnityEditor;


/// <summary>
/// A script required to be attached on the Main Camera in order to simulate effect
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorblindnessSimulation : MonoBehaviour
{
    static bool isSimulationActive = true;
    public bool colorBlindMode;
    [HideInInspector] public Material simulationMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
#if UNITY_EDITOR

        if (simulationMaterial == null)
        {
            simulationMaterial = Resources.Load<Material>("SimulationMaterial");
        }

        // Debug.Log("Render Image called");
        if (!colorBlindMode || !isSimulationActive)
        {
            Graphics.Blit(src, dest);
            return;
        }
        // Debug.Log("Render Image second call");
        Graphics.Blit(src, dest, simulationMaterial);
#endif
    }

    public void SetSimulationConfiguration(ColorblindnessMode _colorblindnessMode)
    {
        simulationMaterial.SetInt("_Type", (int)_colorblindnessMode);
        colorBlindMode = (_colorblindnessMode != ColorblindnessMode.Normal);
        Shader.SetGlobalInteger("_Type", (int)_colorblindnessMode);

        switch (_colorblindnessMode)
        {
            case ColorblindnessMode.Normal:
                Debug.Log("Normal");
                break;
            case ColorblindnessMode.Protanopia:
                Debug.Log("Protonopia");
                break;
            case ColorblindnessMode.Deuteranopia:
                Debug.Log("Deuteranopia");
                break;
            case ColorblindnessMode.Tritanopia:
                Debug.Log("Tritanopia");
                break;
            default:
                break;
        }
    }

    [MenuItem("Visual Accessibility Toolkit/Enable Colorblindness Simulation in Game View")]
    static void EnableSimulation()
    {
        isSimulationActive = true;
        // ColorCorrectionRuntime.DisableCorrection();
    }

    [MenuItem("Visual Accessibility Toolkit/Disable Colorblindness Simulation in Game View")]
    static void DisableSimulation()
    {
        isSimulationActive = false;
        // ColorCorrectionRuntime.EnableCorrection();
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.SetSceneViewShaderReplace(null, null);
        }
        SceneView.RepaintAll();
    }
#endif
}


public enum ColorblindnessMode
{
    Normal = 0,
    Protanopia = 1,
    Deuteranopia = 2,
    Tritanopia = 3
}
