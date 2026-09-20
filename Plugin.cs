using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DragonCliffMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;
        Logger.LogInfo($"龙崖 MOD 加载中... v{MyPluginInfo.PLUGIN_VERSION}");

        try
        {
            Harmony.CreateAndPatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Harmony patch 全部注册完成");
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"Harmony patch 注册失败: {ex.Message}");
        }
    }
}