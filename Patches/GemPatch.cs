using DragonCliffMod.Config;
using HarmonyLib;
using static DragonCliffMod.Plugin;

namespace DragonCliffMod.Patches;

/// <summary>
/// 宝石相关 Harmony patch。
/// </summary>
[HarmonyPatch]
public static class GemPatch
{
    // ─── 宝石等级覆盖 ────────────────────────────────────────

    /// <summary>
    /// Hook 宝石掉落等级，返回配置值。
    /// </summary>
    [HarmonyPatch(typeof(GemGeneratorBase), "GetGemLevel")]
    [HarmonyPrefix]
    static bool OverrideGemLevel(ref int __result)
    {
        var overrideLevel = ModConfig.GemLevelOverride.Value;
        if (overrideLevel <= 0) return true; // 继续执行原方法

        __result = overrideLevel;
        Log?.LogDebug($"[Gem] Level override → {overrideLevel}");
        return false; // 跳过原方法
    }

    // ─── 宝石属性倍率 ────────────────────────────────────────

    /// <summary>
    /// Hook 宝石随机属性系数，乘以倍率。
    /// </summary>
    [HarmonyPatch(typeof(GemGeneratorBase), "GetRandomCoeff")]
    [HarmonyPostfix]
    static void ScaleGemStats(ref double __result)
    {
        var mult = ModConfig.GemStatMultiplier.Value;
        if (mult.Approx(1.0f)) return;
        __result *= mult;
        Log?.LogDebug($"[Gem] GetRandomCoeff scaled: {__result:F2}");
    }
}