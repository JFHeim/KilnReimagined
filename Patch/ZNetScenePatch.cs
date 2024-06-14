namespace KilnReimagined.Patch;

[HarmonyPatch]
public class ZNetScenePatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    public static void Patch()
    {
        var piece = Kiln.Prefab.GetComponent<Piece>();
        var wearNTear = Kiln.Prefab.GetComponent<WearNTear>();
        var smelter = Kiln.Prefab.GetComponent<Smelter>();
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
        Kiln.Prefab.transform.FindChildByName("SmokeSpawner").GetComponent<SmokeSpawner>().m_smokePrefab =
            orig.transform.FindChildByName("SmokeSpawner").GetComponent<SmokeSpawner>().m_smokePrefab;

        FixParticle("smoke (1)");
        FixParticle("flames");
        FixParticle("flare");
        FixSfx("SFX");

        void FixParticle(string name)
        {
            Kiln.Prefab.transform.FindChildByName(name).gameObject.GetComponent<ParticleSystemRenderer>().materials =
                orig.transform.FindChildByName(name).gameObject.GetComponent<ParticleSystemRenderer>().materials;
        }

        void FixSfx(string name)
        {
            var mine = Kiln.Prefab.transform.FindChildByName(name).gameObject;
            var original = orig.transform.FindChildByName(name).gameObject;
            mine.GetComponent<ZSFX>().m_audioClips = original.GetComponent<ZSFX>().m_audioClips;

            mine.GetComponent<AudioSource>().outputAudioMixerGroup =
                original.GetComponent<AudioSource>().outputAudioMixerGroup;
        }

        if (RemoveOriginalFromHammerConfig.Value)
            ZNetScene.instance.GetItem("Hammer").m_itemData.m_shared.m_buildPieces.m_pieces.Remove(orig);
    }
}