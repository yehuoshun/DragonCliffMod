using BepInEx.Configuration;

namespace DragonCliffMod
{
    /// <summary>
    /// 插件配置入口。
    /// 所有用户可调参数集中管理，运行时自动生成 cfg 文件。
    /// </summary>
    public static class ModConfig
    {
        // ─── 装备 ────────────────────────────────────────────
        public static ConfigEntry<bool>  ForceAncient        { get; private set; }
        public static ConfigEntry<float> EquipStatMultiplier { get; private set; }
        public static ConfigEntry<int>   MaxSockets          { get; private set; }

        // ─── 宝石 ────────────────────────────────────────────
        public static ConfigEntry<float> GemStatMultiplier   { get; private set; }
        public static ConfigEntry<int>   GemMaxLevel         { get; private set; }

        // ─── 强化 ────────────────────────────────────────────
        public static ConfigEntry<bool>  FreeUpgrade         { get; private set; }
        public static ConfigEntry<float> UpgradeSuccessRate  { get; private set; }
        public static ConfigEntry<bool>  NoUpgradeCap        { get; private set; }

        // ─── 技能/等级 ──────────────────────────────────────
        public static ConfigEntry<int>   LevelCapOverride    { get; private set; }

        // ─── 招募 ────────────────────────────────────────────
        public static ConfigEntry<bool>  ForceStarAdventurer { get; private set; }
        public static ConfigEntry<bool>  FullTalentAdventurer { get; private set; }

        /// <summary>
        /// 在 Plugin.Awake() 中调用，绑定所有配置项。
        /// </summary>
        public static void Initialize(ConfigFile config)
        {
            ForceAncient        = config.Bind("装备", "必远古",       true,  "所有装备必出远古品质");
            EquipStatMultiplier = config.Bind("装备", "属性倍率",     1.0f,  "装备属性倍率（1=原版）");
            MaxSockets          = config.Bind("装备", "最大打孔数",   0,     "装备最大打孔数（0=原版4/3，>0自定义）");

            GemStatMultiplier   = config.Bind("宝石", "属性倍率",     1.0f,  "宝石属性倍率（1=原版）");
            GemMaxLevel         = config.Bind("宝石", "最大等级",     0,     "宝石等级上限（0=原版25）");

            FreeUpgrade         = config.Bind("强化", "免费强化",     false, "强化不消耗素材和金币");
            UpgradeSuccessRate  = config.Bind("强化", "成功率倍率",   1.0f,  "强化成功率倍率（1=原版）");
            NoUpgradeCap        = config.Bind("强化", "取消等级上限", false, "取消强化等级上限（原版100级）");

            LevelCapOverride    = config.Bind("技能", "等级上限覆盖", 0,     "英雄等级上限（0=原版90级，需反编译 firstpass）");

            ForceStarAdventurer = config.Bind("招募", "星辰冒险者", false, "酒馆必出星辰冒险者");
            FullTalentAdventurer = config.Bind("招募", "满天赋",     false, "招募的冒险者天赋点满(17点)");
        }
    }
}