using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServerSync;
using UnityEngine;
using PieceManager;

namespace KilnReimagined;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    internal const string ModName = "Frogger.KilnReimagined",
        ModAuthor = "Frogger",
        ModVersion = "1.3.0",
        ModGUID = "com." + ModName;

    internal static BuildPiece kiln;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion);

        kiln = new("kiln", "JF_KilnReimagined");
        kiln.Crafting.Set(CraftingTable.Workbench);
        kiln.Category.Set(BuildPieceCategory.Crafting);
        kiln.RequiredItems.Add("Stone", 20, true);
        kiln.RequiredItems.Add("SurtlingCore", 20, true);
    }
}