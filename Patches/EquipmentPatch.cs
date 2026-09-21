using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 装备相关 Harmony patch（已对接真实反编译方法）。
    /// </summary>
    public static class EquipmentPatch
    {
        // ─── 必远古：随机品质入口 ──────────────────────────────
        // GenerationDistribution.GetGrade() → QualityGrade
        // 覆盖：生产(Recipe)/掉落/商店 等走随机品质的场景
        [HarmonyPatch(typeof(GenerationDistribution), "GetGrade")]
        [HarmonyPostfix]
        static void ForceAncient(ref QualityGrade __result)
        {
            if (!ModConfig.ForceAncient.Value) return;
            __result = QualityGrade.Ancient;
        }

        // ─── 必星辰 ──────────────────────────────────────────────
        // DifficultyLevelMeasurement.GetStarChance(ResourceSourceType) → double
        // 教程做法：return 1.0，锻造出星辰装备（远古+特效）
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