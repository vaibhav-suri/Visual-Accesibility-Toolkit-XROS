using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// A script to apply the Color Correction Shader on Main Camera
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorCorrectionRuntime : MonoBehaviour
{
    Material correctionMaterial = null;
    public ColorCorrectionMode type = ColorCorrectionMode.Normal;
    [Range(0, 1)]
    public float intensity = 1f;
    static bool isCorrectionEnabled = true;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isCorrectionEnabled)
        {
            if (correctionMaterial == null)
            {
                correctionMaterial = Resources.Load<Material>("CorrectionMaterial");
            }

            RenderTextureDescriptor desc;
            if (XRSettings.enabled)
                desc = XRSettings.eyeTextureDesc;
            else
                desc = new RenderTextureDescriptor(Screen.width, Screen.height); // Not XR
            correctionMaterial.SetFloat("_Intensity", intensity);
            correctionMaterial.SetInt("_CorrectionType", (int)type);
            RenderTexture rt = RenderTexture.GetTemporary(desc);
            Graphics.Blit(source, rt, correctionMaterial, 0);
            Graphics.Blit(rt, destination, correctionMaterial, 1);
            RenderTexture.ReleaseTemporary(rt);
        }
    }

    public static void EnableCorrection()
    {
        isCorrectionEnabled = true;
    }

    public static void DisableCorrection()
    {
        isCorrectionEnabled = false;
    }
}


public enum ColorCorrectionMode
{
    Normal = 0,
    Protanopia = 1,
    Deuteranopia = 2,
    Tritanopia = 3
}