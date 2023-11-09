namespace KilnReimagined;

[HarmonyPatch]
public class ZNetScenePatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))] [HarmonyPostfix] [HarmonyWrapSafe]
    public static void Patch(ZNetScene __instance)
    {
        var piece = kiln.Prefab.GetComponent<Piece>();
        var wearNTear = kiln.Prefab.GetComponent<WearNTear>();
        var smelter = kiln.Prefab.GetComponent<Smelter>();
        var orig = ZNetScene.instance.GetPrefab("charcoal_kiln");
        var origSmelter = ZNetScene.instance.GetPrefab("charcoal_kiln").GetComponent<Smelter>();
        var origPiece = orig.GetComponent<Piece>();
        var origWearNTear = orig.GetComponent<WearNTear>();
        piece.m_placeEffect = origPiece.m_placeEffect;
        wearNTear.m_destroyedEffect = origWearNTear.m_destroyedEffect;
        wearNTear.m_hitEffect = origWearNTear.m_hitEffect;
        wearNTear.m_switchEffect = origWearNTear.m_switchEffect;
        smelter.m_conversion = origSmelter.m_conversion;
        smelter.m_produceEffects = origSmelter.m_produceEffects;
        smelter.m_fuelAddedEffects = origSmelter.m_fuelAddedEffects;
        smelter.m_oreAddedEffects = origSmelter.m_oreAddedEffects;
        smelter.m_maxOre = origSmelter.m_maxOre;
        smelter.m_maxFuel = origSmelter.m_maxFuel;
        Utils.FindChild(kiln.Prefab.transform, "SmokeSpawner").gameObject.GetComponent<SmokeSpawner>().m_smokePrefab =
            Utils.FindChild(orig.transform, "SmokeSpawner").gameObject.GetComponent<SmokeSpawner>().m_smokePrefab;

        Materials.FixRenderers(kiln.Prefab);
    }
}