using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DragonCliffMod
{
    public static class MyPluginInfo
    {
        public const string PLUGIN_GUID = "yehuoshun.DragonCliffMod";
        public const string PLUGIN_NAME = "DragonCliffMod";
        public const string PLUGIN_VERSION = "1.0.0";
    }

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Logger.LogInfo("龙崖 MOD 加载中... v" + MyPluginInfo.PLUGIN_VERSION);

            // 绑定配置项（自动生成 BepInEx/config/yehuoshun.DragonCliffMod.cfg）
            ModConfig.Initialize(Config);

            // 反射修改游戏里的 static readonly 字段（如宝石等级上限）
            ApplyStaticOverrides();

            try
            {
                var harmony = new HarmonyLib.Harmony(MyPluginInfo.PLUGIN_GUID);
                harmony.PatchAll(typeof(Plugin).Assembly);
                Logger.LogInfo("Harmony patch 全部注册完成");
            }
            catch (Exception ex)
            {
                Logger.LogError("Harmony patch 注册失败: " + ex);
            }
        }

        /// <summary>
        /// 用反射修改游戏里无法通过 Harmony 直接改的 static readonly 字段。
        /// </summary>
        private static void ApplyStaticOverrides()
        {
            // 宝石等级上限：ItemExtensions.MaxGemLevel = 25 → 自定义
            int maxGem = ModConfig.GemMaxLevel.Value;
            if (maxGem > 0)
            {
                try
                {
                    FieldInfo f = typeof(ItemExtensions).GetField("MaxGemLevel",
                        BindingFlags.Public | BindingFlags.Static);
                    if (f != null)
                    {
                        f.SetValue(null, maxGem);
                        Logger.LogInfo("宝石等级上限已改为 " + maxGem);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("修改 MaxGemLevel 失败: " + ex);
                }
            }
        }
    }
}