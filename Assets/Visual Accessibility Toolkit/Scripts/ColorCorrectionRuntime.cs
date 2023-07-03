using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorCorrectionRuntime : MonoBehaviour
{
     Material correctionMaterial = null;
    [Range(0, 3)]
    public int type = 0;
    [Range(0, 1)]
    public float intensity = 0.5f;

     
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (correctionMaterial==null)
        {
            correctionMaterial = Resources.Load<Material>("CorrectionMaterial");
        }

        RenderTextureDescriptor desc;
        if (XRSettings.enabled)
            desc = XRSettings.eyeTextureDesc;
        else
            desc = new RenderTextureDescriptor(Screen.width, Screen.height); // Not XR
        correctionMaterial.SetFloat("_Intensity", intensity);
        correctionMaterial.SetInt("_Type", type);
        RenderTexture rt = RenderTexture.GetTemporary(desc);
        Graphics.Blit(source, rt, correctionMaterial, 0);
        Graphics.Blit(rt, destination, correctionMaterial, 1);
        RenderTexture.ReleaseTemporary(rt);
    }

}