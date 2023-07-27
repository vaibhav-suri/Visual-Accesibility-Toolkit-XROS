using UnityEngine;
using UnityEditor;

/// <summary>
/// A script used to simulate colorblindness shader in Scene View
/// </summary>
public class SceneViewSimulator : MonoBehaviour
{
    const string ReplacementShaderName = "Resources/Shaders/Colorblind";
    static Shader s_shader;
    
    protected static Shader ReplacementShader {
        get { return s_shader = s_shader ?? Shader.Find(ReplacementShaderName); }
    }

    [MenuItem("Visual Accessibility Toolkit/Simulate Colorblindness in Scene View")]
    static void SceneViewCustomSceneMode()
    {
        // s_shader = Shader.Find(ReplacementShaderName);
        s_shader = Resources.Load<Shader>("Shaders/Colorblind");
        if (s_shader != null)
        {
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                sceneView.SetSceneViewShaderReplace(ReplacementShader, null);
            }
        }
        SceneView.RepaintAll();
        Debug.Log("Color Simulation is enabled on Scene View");
    }

    [MenuItem("Visual Accessibility Toolkit/Clear Scene View")]
    static void SceneViewClearSceneView()
    {
        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.SetSceneViewShaderReplace(null, null);
        }
        SceneView.RepaintAll();
        Debug.Log("Color Simulation is disabled on Scene View");
    }
}
