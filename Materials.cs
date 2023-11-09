#nullable enable
namespace KilnReimagined;

[HarmonyPatch]
public static class Materials
{
    public static void FixRenderers(GameObject asset)
    {
        var renderers = asset.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) return;

        foreach (var renderer in renderers)
        {
            if (!renderer) continue;
            foreach (var material in renderer.sharedMaterials)
            {
                if (!material) continue;
                var shader = material.shader;
                if (!shader) return;
                var name = shader.name;
                material.shader = Shader.Find(name);
            }
        }
    }
}