using BepInEx;
using BepInEx.Configuration;
using PieceManager;
using MaterialReplacer = KilnReimagined.PieceManager.MaterialReplacer;

namespace KilnReimagined;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    internal const string ModName = "Frogger.KilnReimagined",
        ModAuthor = "Frogger",
        ModVersion = "1.4.0",
        ModGUID = $"com.{ModAuthor}.{ModName}";
 
    public static BuildPiece KilnPiece = null!;
    public static ConfigEntry<bool> RemoveOriginalFromHammerConfig = null!;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);
        RemoveOriginalFromHammerConfig = config("General", "Remove Original From Hammer", true,
            "<color=red>Requires full game restart</color> Remove the original Kiln from the hammer?");

        OnConfigurationChanged += () =>
        {
            LogInfo("Configuration Changed");
            if (!ZNetScene.instance) return;

            var pieceTable = ZNetScene.instance.GetItem("Hammer")?.m_itemData.m_shared.m_buildPieces;
            var orig = pieceTable ? ZNetScene.instance.GetPrefab("charcoal_kiln") : null;
            if(!pieceTable || !orig) return;
            if (RemoveOriginalFromHammerConfig.Value) pieceTable.m_pieces.Remove(orig);
            else if (!pieceTable.m_pieces.Contains(orig)) pieceTable.m_pieces.Add(orig);
        };

        LoadAssetBundle("kiln");
 
        KilnPiece = new BuildPiece(bundle, "JF_KilnReimagined");
        KilnPiece.Crafting.Set(CraftingTable.Workbench);
        KilnPiece.Category.Set(BuildPieceCategory.Crafting);
        KilnPiece.RequiredItems.Add("Stone", 20, true);
        KilnPiece.RequiredItems.Add("SurtlingCore", 20, true);
        MaterialReplacer.RegisterGameObjectForShaderSwap(KilnPiece.Prefab, MaterialReplacer.ShaderType.UseUnityShader);
    }
}