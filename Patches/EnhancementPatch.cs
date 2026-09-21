using System.Collections.Generic;
using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 强化相关 Harmony patch 示例。
    /// 用法同 EquipmentPatch：反编译确认真实类名后启用。
    /// </summary>
    public static class EnhancementPatch
    {
#if false
        // ─── 免费强化 ────────────────────────────────────────────
        // 目标：强化素材需求改为 0
        [HarmonyPatch(typeof(ItemExtensions), "AmountRequired")]
        [HarmonyPrefix]
        static bool FreeUpgrade(ref int __result)
        {
            if (!ModConfig.FreeUpgrade.Value) return true;
            __result = 0;
            return false;
        }

        // ─── 强化成功率 ──────────────────────────────────────────
        // 目标：成功率乘以配置倍率
        [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "GetTeamSetUpgradeSuccessChance")]
        [HarmonyPostfix]
        static void ScaleSuccessRate(ref float __result)
        {
            float mult = ModConfig.UpgradeSuccessRate.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
        }

        // ─── 取消强化等级上限 ────────────────────────────────────
        // 目标：强化等级上限判定永远为 true
        [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "CanTeamSetUpgrade")]
        [HarmonyPostfix]
        static void RemoveUpgradeCap(ref bool __result)
        {
            if (!ModConfig.NoUpgradeCap.Value) return;
            __result = true;
        }

        // ─── 饰品/套装强化素材需求 ───────────────────────────────
        // 目标：免费强化时清空素材列表
        [HarmonyPatch(typeof(TeamSetUpgradeRequirements), "GetTeamSetUpgradeRequirements")]
        [HarmonyPrefix]
        static bool FreeTeamSetUpgrade(ref List<ItemAmount> __result)
        {
            if (!ModConfig.FreeUpgrade.Value) return true;
            __result = new List<ItemAmount>();
            return false;
        }
#endif
    }
}