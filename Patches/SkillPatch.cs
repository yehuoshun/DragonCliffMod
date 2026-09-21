using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 技能/等级相关 Harmony patch 示例。
    /// 用法同 EquipmentPatch：反编译确认真实类名后启用。
    /// </summary>
    public static class SkillPatch
    {
#if false
        // ─── 等级上限覆盖 ────────────────────────────────────────
        // 目标：英雄等级上限（原版 90 级）改为配置值
        // 注意：实际类名/方法名需根据反编译结果替换，原版可能无独立 GetMaxLevel 方法
        [HarmonyPatch(typeof(UnitGrowthProfile), "GetMaxLevel")]
        [HarmonyPrefix]
        static bool OverrideLevelCap(ref int __result)
        {
            int cap = ModConfig.LevelCapOverride.Value;
            if (cap <= 0) return true;

            __result = cap;
            return false;
        }
#endif
    }
}