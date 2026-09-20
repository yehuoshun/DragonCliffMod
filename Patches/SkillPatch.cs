using DragonCliffMod.Config;
using HarmonyLib;
using static DragonCliffMod.Plugin;

namespace DragonCliffMod.Patches;

/// <summary>
/// 技能/等级相关 Harmony patch。
/// </summary>
[HarmonyPatch]
public static class SkillPatch
{
    // ─── 等级上限覆盖 ────────────────────────────────────────

    /// <summary>
    /// Hook 等级上限判定，返回配置值。
    /// 实际类名和方法名需根据反编译结果替换。
    /// </summary>
    [HarmonyPatch(typeof(UnitGrowthProfile), "GetMaxLevel")]
    [HarmonyPrefix]
    static bool OverrideLevelCap(ref int __result)
    {
        var cap = ModConfig.LevelCapOverride.Value;
        if (cap <= 0) return true;

        __result = cap;
        Log?.LogDebug($"[Skill] Level cap override → {cap}");
        return false;
    }
}