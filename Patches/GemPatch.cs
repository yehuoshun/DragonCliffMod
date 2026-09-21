using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 宝石相关 Harmony patch 示例。
    /// 用法同 EquipmentPatch：反编译确认真实类名后启用。
    /// </summary>
    public static class GemPatch
    {
#if false
        // ─── 宝石等级覆盖 ────────────────────────────────────────
        // 目标：强制宝石掉落等级
        [HarmonyPatch(typeof(GemGeneratorBase), "GetGemLevel")]
        [HarmonyPrefix]
        static bool OverrideGemLevel(ref int __result)
        {
            int lv = ModConfig.GemLevelOverride.Value;
            if (lv <= 0) return true; // 继续执行原方法

            __result = lv;
            return false; // 跳过原方法
        }

        // ─── 宝石属性倍率 ────────────────────────────────────────
        // 目标：宝石随机属性系数乘以倍率
        [HarmonyPatch(typeof(GemGeneratorBase), "GetRandomCoeff")]
        [HarmonyPostfix]
        static void ScaleGemStats(ref double __result)
        {
            float mult = ModConfig.GemStatMultiplier.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }
#endif
    }
}