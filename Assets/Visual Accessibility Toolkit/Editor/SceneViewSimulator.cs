using UnityEngine;
using UnityEditor;

public class SceneViewSimulator : MonoBehaviour
{
    const string ReplacementShaderName = "Custom/Colorblind";
    static Shader s_shader;

    [MenuItem("VisualToolkit/Custom Render Mode on SceneView")]
    static void SceneViewCustomSceneMode()
    {
        s_shader = Shader.Find(ReplacementShaderName);
        if (s_shader != null)
        {
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                sceneView.SetSceneViewShaderReplace(s_shader, null);
            }
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
