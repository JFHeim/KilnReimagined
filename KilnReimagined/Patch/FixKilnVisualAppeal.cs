namespace KilnReimagined.Patch;

[HarmonyPatch, HarmonyWrapSafe]
file static class FixKilnVisualAppeal
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static void Postfix()
    {
        var piece = KilnPiece.Prefab.GetComponent<Piece>();
        var wearNTear = KilnPiece.Prefab.GetComponent<WearNTear>();
        var smelter = KilnPiece.Prefab.GetComponent<Smelter>();
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
        {
            var modsSmokeSpawner = KilnPiece.Prefab.transform.FindChildByName("SmokeSpawner")?.GetComponent<SmokeSpawner>();
            var origSmokeSpawner = modsSmokeSpawner ? orig.transform.FindChildByName("SmokeSpawner")?.GetComponent<SmokeSpawner>() : null;
            if (modsSmokeSpawner && origSmokeSpawner) 
                modsSmokeSpawner.m_smokePrefab = origSmokeSpawner.m_smokePrefab;
        }

        FixParticle("smoke (1)");
        FixParticle("flames");
        FixParticle("flare");
        FixSfx("SFX");

        if (RemoveOriginalFromHammerConfig.Value && ZNetScene.instance)
            ZNetScene.instance.GetItem("Hammer")?.m_itemData.m_shared.m_buildPieces.m_pieces.Remove(orig);

        FixLavaPlane();
        
        return;

        void FixParticle(string name)
        {
            var modsRenderer = KilnPiece.Prefab.transform.FindChildByName(name)?.GetComponent<ParticleSystemRenderer>();
            var origRenderer = modsRenderer ? orig.transform.FindChildByName(name)?.GetComponent<ParticleSystemRenderer>() : null;
            if (!modsRenderer || !origRenderer) return;
            modsRenderer.materials = origRenderer.materials;
        }

        void FixSfx(string name)
        {
            var modsSfx = KilnPiece.Prefab.transform.FindChildByName(name)?.GetComponent<ZSFX>();
            var origSfx = modsSfx ? orig.transform.FindChildByName(name)?.GetComponent<ZSFX>() : null;
            if (!modsSfx || !origSfx) return;
            modsSfx.m_audioClips = origSfx.GetComponent<ZSFX>().m_audioClips;

            if(!modsSfx.m_audioSource || !origSfx.m_audioSource) return;
            modsSfx.m_audioSource.outputAudioMixerGroup = origSfx.m_audioSource.outputAudioMixerGroup;
        }

        void FixLavaPlane()
        {
            var standardShader = ZNetScene.instance?.GetPrefab("Amber")?
                .transform.Find("attach/model")?
                .GetComponent<MeshRenderer>()?
                .sharedMaterial?.shader;
            var lavaPlane = standardShader
                ? KilnPiece.Prefab.transform.FindChildByName("JF_Lava_Plane")?.GetComponent<MeshRenderer>()
                : null;
            
            if(!standardShader || !lavaPlane) return;
            lavaPlane.sharedMaterial.shader = standardShader;
        }
    }
}