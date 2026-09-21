using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 宝石相关 Harmony patch（已对接真实反编译方法）。
    /// </summary>
    public static class GemPatch
    {
        // ─── 宝石属性倍率 ────────────────────────────────────────
        // GemGeneratorBase.GetRandomCoeff() (private) → double
        // 原逻辑: Random.Range(1 - GemAttributeRandomness, 1)，默认 0.8~1.0
        [HarmonyPatch(typeof(GemGeneratorBase), "GetRandomCoeff")]
        [HarmonyPostfix]
        static void ScaleGemStats(ref double __result)
        {
            float mult = ModConfig.GemStatMultiplier.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }
    }
}