using BepInEx.Configuration;

namespace DragonCliffMod.Config;

/// <summary>
/// 插件配置入口。
/// 所有用户可调参数集中管理，运行时自动生成 BepInEx/config/*.cfg。
/// </summary>
public static class ModConfig
{
    // ─── 装备 ────────────────────────────────────────────────

    public static ConfigEntry<bool>   ForceStarChance      { get; private set; } = null!;
    public static ConfigEntry<float>  EquipStatMultiplier  { get; private set; } = null!;
    public static ConfigEntry<int>    MaxSockets           { get; private set; } = null!;

    // ─── 宝石 ────────────────────────────────────────────────

    public static ConfigEntry<int>    GemLevelOverride     { get; private set; } = null!;
    public static ConfigEntry<float>  GemStatMultiplier    { get; private set; } = null!;

    // ─── 强化 ────────────────────────────────────────────────

    public static ConfigEntry<bool>   FreeUpgrade          { get; private set; } = null!;
    public static ConfigEntry<float>  UpgradeSuccessRate   { get; private set; } = null!;
    public static ConfigEntry<bool>   NoUpgradeCap         { get; private set; } = null!;

    // ─── 技能/等级 ──────────────────────────────────────────

    public static ConfigEntry<int>    LevelCapOverride     { get; private set; } = null!;

    /// <summary>
    /// 在 Plugin.Awake() 中调用，绑定所有配置项。
    /// </summary>
    public static void Initialize(ConfigFile config)
    {
        // 装备
        ForceStarChance     = config.Bind("装备", "强制星辰",       false, "所有装备必出星辰品质");
        EquipStatMultiplier = config.Bind("装备", "属性倍率",       1.0f,  "装备属性倍率（1=原版）");
        MaxSockets          = config.Bind("装备", "最大打孔数",     10,    "装备最大打孔数（原版=1）");

        // 宝石
        GemLevelOverride    = config.Bind("宝石", "等级覆盖",       0,     "0=不覆盖, >0 强制宝石等级");
        GemStatMultiplier   = config.Bind("宝石", "属性倍率",       1.0f,  "宝石属性倍率（1=原版）");

        // 强化
        FreeUpgrade         = config.Bind("强化", "免费强化",       false, "强化不消耗素材和金币");
        UpgradeSuccessRate  = config.Bind("强化", "成功率倍率",     1.0f,  "强化成功率倍率（1=原版）");
        NoUpgradeCap        = config.Bind("强化", "取消等级上限",   false, "取消强化等级上限");

        // 技能/等级
        LevelCapOverride    = config.Bind("技能", "等级上限覆盖",   0,     "0=原版90级, >0 自定义上限");
    }
}