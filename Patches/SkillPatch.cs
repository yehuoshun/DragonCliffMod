using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 技能/英雄等级上限。
    ///
    /// ✅ 已实现：通过 Plugin.ApplyLevelCapOverride() 用反射改
    ///   UnitExtensions.MaxAdventurerLevel（static int = 90）+ 补 LevelConfigs 经验表。
    ///   不需要 Harmony patch，此处仅保留说明占位。
    ///
    /// 配置项：ModConfig.LevelCapOverride（0=原版，>90 生效）
    /// </summary>
    public static class SkillPatch
    {
        // 无需 Harmony patch，等级上限由反射处理。
    }
}