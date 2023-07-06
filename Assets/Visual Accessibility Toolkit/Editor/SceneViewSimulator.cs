using UnityEngine;
using UnityEditor;

public class SceneViewSimulator : MonoBehaviour
{
    const string ReplacementShaderName = "Resources/Shaders/Colorblind";
    static Shader s_shader;
    
    protected static Shader ReplacementShader {
        get { return s_shader = s_shader ?? Shader.Find(ReplacementShaderName); }
    }

    [MenuItem("VisualToolkit/Custom Render Mode on SceneView")]
    static void SceneViewCustomSceneMode()
    {
        // s_shader = Shader.Find(ReplacementShaderName);
        s_shader = Resources.Load<Shader>("Shaders/Colorblind");
        Debug.Log(ReplacementShader);
        if (s_shader != null)
        {
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                sceneView.SetSceneViewShaderReplace(ReplacementShader, null);
            }
            Debug.Log("Shader attached");
        }
        SceneView.RepaintAll();
    }

    [MenuItem("VisualToolkit/Clear SceneView")]
    static void SceneViewClearSceneView()
    {
        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.SetSceneViewShaderReplace(null, null);
        }
        SceneView.RepaintAll();
    }
}
