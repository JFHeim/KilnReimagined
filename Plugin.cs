using BepInEx;
using PieceManager;

namespace KilnReimagined;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    internal const string ModName = "Frogger.KilnReimagined",
        ModAuthor = "Frogger",
        ModVersion = "1.3.1",
        ModGUID = "com." + ModName;

    internal static BuildPiece kiln;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);

        kiln = new BuildPiece("kiln", "JF_KilnReimagined");
        kiln.Crafting.Set(CraftingTable.Workbench);
        kiln.Category.Set(BuildPieceCategory.Crafting);
        kiln.RequiredItems.Add("Stone", 20, true);
        kiln.RequiredItems.Add("SurtlingCore", 20, true);
    }
}