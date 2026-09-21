using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 装备相关 Harmony patch（已对接真实反编译方法）。
    /// </summary>
    public static class EquipmentPatch
    {
        // ─── 必星辰 ──────────────────────────────────────────────
        // DifficultyLevelMeasurement.GetStarChance(ResourceSourceType) → double
        [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetStarChance")]
        [HarmonyPostfix]
        static void ForceStar(ref double __result)
        {
            if (!ModConfig.ForceStarChance.Value) return;
            __result = 1.0;
        }

        // ─── 装备属性倍率 ────────────────────────────────────────
        // AttributePotentialDescriptor.GetMean(ItemRoot, AttributeGrade) → double
        [HarmonyPatch(typeof(AttributePotentialDescriptor), "GetMean")]
        [HarmonyPostfix]
        static void ScaleEquipStats(ref double __result)
        {
            float mult = ModConfig.EquipStatMultiplier.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }
    }
}