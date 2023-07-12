using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace KilnReimagined;

[HarmonyPatch]
public static class Materials
{
    public static void FixRenderers(GameObject asset)
    {
        var renderers = asset.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) return;

        foreach (Renderer? renderer in renderers)
        {
            if (!renderer) continue;
            foreach (Material? material in renderer.sharedMaterials)
            {
                if (!material) continue;
                var shader = material.shader;
                if (!shader) return;
                string name = shader.name;
                material.shader = Shader.Find(name);
            }
        }
    }
}