using HarmonyLib;

namespace DragonCliffMod.Patches
{
    // ─── 必远古：随机品质入口 ──────────────────────────────
    // Prefix 跳过原方法，直接 return Ancient —— 等效于教程"改方法体 return Ancient"
    [HarmonyPatch(typeof(GenerationDistribution), "GetGrade")]
    public static class ForceAncientPatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref QualityGrade __result)
        {
            if (!ModConfig.ForceAncient.Value) return true; // 不开启则走原逻辑
            __result = QualityGrade.Ancient;
            return false; // 跳过原方法体
        }
    }

    // ─── 必星辰 ──────────────────────────────────────────────
    // Prefix 跳过原方法，直接 return 1.0 —— 等效于教程"return 1.0"
    [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetStarChance")]
    public static class ForceStarPatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref double __result)
        {
            if (!ModConfig.ForceStarChance.Value) return true;
            __result = 1.0;
            return false;
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