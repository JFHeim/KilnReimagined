using BepInEx;
using BepInEx.Configuration;
using KilnReimagined.PieceManager;

namespace KilnReimagined;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    internal const string ModName = "Frogger.KilnReimagined",
        ModAuthor = "Frogger",
        ModVersion = "1.3.7",
        ModGUID = $"com.{ModAuthor}.{ModName}";
 
    public static BuildPiece Kiln;

    public static ConfigEntry<bool> RemoveOriginalFromHammerConfig;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);
        RemoveOriginalFromHammerConfig = config("General", "Remove Original From Hammer", true,
            "<color=red>Requires full game restart</color> Remove the original Kiln from the hammer?");

        OnConfigurationChanged += () =>
        {
            Debug($"Configuration Changed");
            if (!ZNetScene.instance) return;

            var pieceTable = ZNetScene.instance.GetItem("Hammer").m_itemData.m_shared.m_buildPieces;
            var orig = ZNetScene.instance.GetPrefab("charcoal_kiln");
            if (RemoveOriginalFromHammerConfig.Value) pieceTable.m_pieces.Remove(orig);
            else if (!pieceTable.m_pieces.Contains(orig)) pieceTable.m_pieces.Add(orig);
        };

        LoadAssetBundle("kiln");
 
        Kiln = new BuildPiece(bundle, "JF_KilnReimagined");
        Kiln.Crafting.Set(CraftingTable.Workbench);
        Kiln.Category.Set(BuildPieceCategory.Crafting);
        Kiln.RequiredItems.Add("Stone", 20, true);
        Kiln.RequiredItems.Add("SurtlingCore", 20, true);
        MaterialReplacer.RegisterGameObjectForShaderSwap(Kiln.Prefab, MaterialReplacer.ShaderType.UseUnityShader);
    }
}