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

            try
            {
                var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
                harmony.PatchAll(typeof(Plugin).Assembly);
                Logger.LogInfo("Harmony patch 全部注册完成");
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Harmony patch 注册失败: " + ex);
            }
        }
    }
}