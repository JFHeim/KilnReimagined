using BepInEx;
using BepInEx.Configuration;
using KilnReimagined.PieceManager;

namespace KilnReimagined;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    internal const string ModName = "Frogger.KilnReimagined",
        ModAuthor = "Frogger",
        ModVersion = "1.3.5",
        ModGUID = $"com.{ModAuthor}.{ModName}";

    internal static BuildPiece kiln;

    public static ConfigEntry<bool> removeOriginalFromHammerConfig;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);
        removeOriginalFromHammerConfig = config("General", "Remove Original From Hammer", true,
            "<color=red>Requires full game restart</color> Remove the original Kiln from the hammer?");

        OnConfigurationChanged += () =>
        {
            Debug($"Configuration Changed");
            if (!ZNetScene.instance) return;

            var pieceTable = ZNetScene.instance.GetItem("Hammer").m_itemData.m_shared.m_buildPieces;
            var orig = ZNetScene.instance.GetPrefab("charcoal_kiln");
            if (removeOriginalFromHammerConfig.Value) pieceTable.m_pieces.Remove(orig);
            else if (!pieceTable.m_pieces.Contains(orig)) pieceTable.m_pieces.Add(orig);
        };

        kiln = new BuildPiece("kiln", "JF_KilnReimagined");
        kiln.Crafting.Set(CraftingTable.Workbench);
        kiln.Category.Set(BuildPieceCategory.Crafting);
        kiln.RequiredItems.Add("Stone", 20, true);
        kiln.RequiredItems.Add("SurtlingCore", 20, true);
    }
}