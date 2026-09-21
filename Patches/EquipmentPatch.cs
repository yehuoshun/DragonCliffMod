using HarmonyLib;

namespace DragonCliffMod.Patches
{
    // ─── 必远古：随机品质入口 ──────────────────────────────
    [HarmonyPatch(typeof(GenerationDistribution), "GetGrade")]
    public static class ForceAncientPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref QualityGrade __result)
        {
            if (!ModConfig.ForceAncient.Value) return;
            __result = QualityGrade.Ancient;
        }
    }

    // ─── 必星辰 ──────────────────────────────────────────────
    [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetStarChance")]
    public static class ForceStarPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref double __result)
        {
            if (!ModConfig.ForceStarChance.Value) return;
            __result = 1.0;
        }
    }

    // ─── 装备属性倍率 ────────────────────────────────────────
    [HarmonyPatch(typeof(AttributePotentialDescriptor), "GetMean")]
    public static class ScaleEquipStatsPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref double __result)
        {
            float mult = ModConfig.EquipStatMultiplier.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }
    }
}