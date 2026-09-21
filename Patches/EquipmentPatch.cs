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

        // ─── 必远古：指定品质入口 ──────────────────────────────
        // DifficultyLevelMeasurement.GetItemGenerationQuality(QualityGrade grade, ...)
        // 覆盖：合成(Combine) 等走指定品质的场景
        [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetItemGenerationQuality")]
        [HarmonyPrefix]
        static void ForceAncientGrade(ref QualityGrade grade)
        {
            if (!ModConfig.ForceAncient.Value) return;
            grade = QualityGrade.Ancient;
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