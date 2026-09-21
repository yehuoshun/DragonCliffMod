using System;
using System.Collections;
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

            // 反射修改游戏里的 static 字段（宝石等级上限、英雄等级上限）
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
        /// 用反射修改游戏里无法通过 Harmony 直接改的 static 字段。
        /// </summary>
        private static void ApplyStaticOverrides()
        {
            // ─── 宝石等级上限：ItemExtensions.MaxGemLevel = 25 → 自定义 ───
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

            // ─── 英雄等级上限：MaxAdventurerLevel = 90 → 自定义 ───
            ApplyLevelCapOverride();
        }

        /// <summary>
        /// 突破英雄等级上限（原版 90 级）。
        /// 需要同时做两件事：改 MaxAdventurerLevel + 补 LevelConfigs 经验表，
        /// 否则超过原配置表长度会 IndexOutOfRange 崩溃。
        /// </summary>
        private static void ApplyLevelCapOverride()
        {
            int cap = ModConfig.LevelCapOverride.Value;
            if (cap <= 90) return; // 0=不修改，<=90 无意义

            try
            {
                // 1. 改等级上限
                FieldInfo capField = typeof(UnitExtensions).GetField("MaxAdventurerLevel",
                    BindingFlags.Public | BindingFlags.Static);
                if (capField == null)
                {
                    Logger.LogError("找不到 UnitExtensions.MaxAdventurerLevel 字段");
                    return;
                }
                capField.SetValue(null, cap);

                // 2. 补经验表 LevelConfigs（List<UnitLevelConfiguration>）
                FieldInfo listField = typeof(UnitExtensions).GetField("LevelConfigs",
                    BindingFlags.Public | BindingFlags.Static);
                IList list = (IList)listField.GetValue(null);
                Type elementType = list.GetType().GetGenericArguments()[0];

                int existing = list.Count;               // 原 92 个（Level 1~92）
                int target = Math.Min(cap + 2, 400);     // 补到 cap+2，最多 400 级防溢出

                if (target <= existing) return;

                // 重放 SetupLevelConfigurations 的公式，得到第 existing 级之后的状态
                long num = 150, num2 = 100, num3 = 10000, num4 = -1;
                for (int i = 0; i < existing; i++)
                {
                    num4 += num;
                    if (i < 59) num += num2;
                    else { num += num3; num3 += num3 / 4; }
                }

                // 追加：90 级之后线性外推（固定增量 num），避免原公式指数爆炸导致 int 溢出
                FieldInfo levelF = elementType.GetField("Level");
                FieldInfo fromF = elementType.GetField("FromExp");
                FieldInfo toF = elementType.GetField("ToExp");

                for (int i = existing; i < target; i++)
                {
                    object cfg = Activator.CreateInstance(elementType);
                    SetFieldValue(cfg, levelF, i + 1);
                    SetFieldValue(cfg, fromF, num4 + 1);
                    SetFieldValue(cfg, toF, num4 + num);
                    list.Add(cfg);
                    num4 += num; // 下一级 FromExp = 本级 ToExp + 1
                }

                Logger.LogInfo("英雄等级上限已改为 " + cap + "，经验表扩展到 " + target + " 级");
            }
            catch (Exception ex)
            {
                Logger.LogError("修改等级上限失败: " + ex);
            }
        }

        /// <summary>
        /// 按字段实际类型写入值（兼容 int/long），防止 int 溢出。
        /// </summary>
        private static void SetFieldValue(object obj, FieldInfo field, long value)
        {
            if (field == null) return;
            if (field.FieldType == typeof(int))
                field.SetValue(obj, (int)Math.Min(value, (long)int.MaxValue));
            else if (field.FieldType == typeof(long))
                field.SetValue(obj, value);
            else
                field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
        }
    }
}