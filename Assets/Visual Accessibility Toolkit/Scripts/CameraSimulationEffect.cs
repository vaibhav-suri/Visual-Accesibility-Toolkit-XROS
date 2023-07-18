using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A script required to be attached on the Main Camera in order to simulate effect
/// </summary>
 [ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraSimulationEffect : MonoBehaviour
{

    public bool colorBlindMode;
    public Material simulationMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        #if UNITY_EDITOR

        if (simulationMaterial == null)
        {
            simulationMaterial = Resources.Load<Material>("SimulationMaterial");
        }

        Debug.Log("Render Image called");
        if (!colorBlindMode)
        {
            Graphics.Blit(src, dest);
            return;
        }
        Debug.Log("Render Image second call");
        Graphics.Blit(src, dest, simulationMaterial);
        #endif
    }
    
}

 
