using System.Collections.Generic;
using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 装备相关 Harmony patch 示例。
    ///
    /// 用法：反编译 Assembly-CSharp.dll 确认真实类名/方法名/namespace 后，
    /// 把下方 #if false 改成 #if true（或直接删掉 #if 包裹）即可启用。
    /// </summary>
    public static class EquipmentPatch
    {
#if false
        // ─── 必星辰 ──────────────────────────────────────────────
        // 目标：所有装备必出星辰品质
        [HarmonyPatch(typeof(DifficultyLevelMeasurement), "GetStarChance")]
        [HarmonyPostfix]
        static void ForceStar(ref float __result)
        {
            if (!ModConfig.ForceStarChance.Value) return;
            __result = 1.0f;
        }

        // ─── 装备属性倍率 ────────────────────────────────────────
        // 目标：装备属性乘以配置倍率
        [HarmonyPatch(typeof(AttributePotentialDescriptor), "GetMean")]
        [HarmonyPostfix]
        static void ScaleEquipStats(ref double __result)
        {
            float mult = ModConfig.EquipStatMultiplier.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }

        // ─── 打孔上限 ────────────────────────────────────────────
        // 目标：修改装备最大打孔数
        [HarmonyPatch(typeof(ItemExtensions), "CanAddMoreManualSockets")]
        [HarmonyPostfix]
        static void MaxSockets(ref bool __result, Item __instance)
        {
            int max = ModConfig.MaxSockets.Value;
            if (max <= 0) return;
            __result = __instance.Sockets.Count < max;
        }
#endif
    }
}