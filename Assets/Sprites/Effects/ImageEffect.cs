using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    public Material Effect;
    private float value = 0;
    private bool GO = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, Effect);   
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z)) { GO = true; }
        if (GO && value < 1) { value += (Time.deltaTime/2); }

        Effect.SetFloat("_Cutoff", value);
    }
}
