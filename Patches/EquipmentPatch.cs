using DragonCliffMod.Config;
using HarmonyLib;
using static DragonCliffMod.Plugin;

// ⚠️ Item 等游戏类型的实际 namespace 需根据反编译结果补全

namespace DragonCliffMod.Patches;

/// <summary>
/// 装备相关 Harmony patch。
///
/// 目标类/方法需根据反编译的 Assembly-CSharp.dll 确认后修改。
/// 以下标注为占位示例，不匹配会静默跳过。
/// </summary>
[HarmonyPatch]
public static class EquipmentPatch
{
    // ─── 必星辰 ──────────────────────────────────────────────

    /// <summary>
    /// Hook 星辰判定方法，强制返回 1.0。
    /// 实际类名和方法名请替换。
    /// </summary>
    [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetStarChance")]
    [HarmonyPostfix]
    static void ForceStar(ref float __result)
    {
        if (!ModConfig.ForceStarChance.Value) return;
        __result = 1.0f;
        Log?.LogDebug("[Equipment] ForceStar → 1.0");
    }

    // ─── 装备属性倍率 ────────────────────────────────────────

    /// <summary>
    /// Hook 装备属性生成方法，乘以配置倍率。
    /// 实际类名和方法名请替换。
    /// </summary>
    [HarmonyPatch(typeof(AttributePotentialDescriptor), "GetMean")]
    [HarmonyPostfix]
    static void ScaleEquipStats(ref double __result)
    {
        var mult = ModConfig.EquipStatMultiplier.Value;
        if (mult.Approx(1.0f)) return;
        __result *= mult;
        Log?.LogDebug($"[Equipment] GetMean scaled: {__result:F2}");
    }

    // ─── 打孔上限 ────────────────────────────────────────────

    /// <summary>
    /// Hook 打孔数量判定，改为返回配置值。
    /// 实际类名和方法名请替换。
    /// </summary>
    [HarmonyPatch(typeof(ItemExtensions), "CanAddMoreManualSockets")]
    [HarmonyPostfix]
    static void MaxSockets(ref bool __result, ref Item __instance)
    {
        var max = ModConfig.MaxSockets.Value;
        if (max <= 0) return;
        __result = __instance.Sockets.Count < max;
    }
}