using System.Collections.Generic;
using DragonCliffMod.Config;
using HarmonyLib;
using static DragonCliffMod.Plugin;

namespace DragonCliffMod.Patches;

/// <summary>
/// 强化相关 Harmony patch。
/// </summary>
[HarmonyPatch]
public static class EnhancementPatch
{
    // ─── 免费强化 ────────────────────────────────────────────

    /// <summary>
    /// Hook 强化素材需求，改为 0。
    /// </summary>
    [HarmonyPatch(typeof(ItemExtensions), "AmountRequired")]
    [HarmonyPrefix]
    static bool FreeUpgrade(ref int __result)
    {
        if (!ModConfig.FreeUpgrade.Value) return true;
        __result = 0;
        return false;
    }

    // ─── 强化成功率 ──────────────────────────────────────────

    /// <summary>
    /// Hook 强化成功率判定，乘以配置倍率。
    /// </summary>
    [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "GetTeamSetUpgradeSuccessChance")]
    [HarmonyPostfix]
    static void ScaleSuccessRate(ref float __result)
    {
        var mult = ModConfig.UpgradeSuccessRate.Value;
        if (mult.Approx(1.0f)) return;
        __result *= mult;
        Log?.LogDebug($"[Enhance] Success rate scaled: {__result:F3}");
    }

    // ─── 取消强化等级上限 ────────────────────────────────────

    /// <summary>
    /// Hook 强化等级上限判定，永远返回 true。
    /// </summary>
    [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "CanTeamSetUpgrade")]
    [HarmonyPostfix]
    static void RemoveUpgradeCap(ref bool __result)
    {
        if (!ModConfig.NoUpgradeCap.Value) return;
        __result = true;
    }

    // ─── 饰品强化素材需求 ────────────────────────────────────

    /// <summary>
    /// Hook 饰品/套装强化素材需求。类名待定。
    /// </summary>
    [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "GetTeamSetUpgradeRequirements")]
    [HarmonyPrefix]
    static bool FreeTeamSetUpgrade(ref List<ItemAmount> __result)
    {
        if (!ModConfig.FreeUpgrade.Value) return true;
        __result = new List<ItemAmount>();
        return false;
    }
}