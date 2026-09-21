using System.Collections.Generic;
using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 强化相关 Harmony patch（已对接真实反编译方法）。
    /// </summary>
    public static class EnhancementPatch
    {
        // ─── 免费强化 ────────────────────────────────────────────
        // Item.TeamSetUpgradeRequirements() → List&lt;ResourceConsumptionRequirement&gt;
        // 清空素材需求列表，MetRequirements() 对空集合返回 true → 免费
        [HarmonyPatch(typeof(Item), "TeamSetUpgradeRequirements")]
        [HarmonyPostfix]
        static void FreeUpgrade(ref List<ResourceConsumptionRequirement> __result)
        {
            if (!ModConfig.FreeUpgrade.Value) return;
            __result = new List<ResourceConsumptionRequirement>();
        }

        // ─── 强化成功率 ──────────────────────────────────────────
        // Item.GetTeamSetUpgradeSuccessChance() → double
        // 原逻辑: (100 - 强化印记长度)/100，最低 0.5
        [HarmonyPatch(typeof(Item), "GetTeamSetUpgradeSuccessChance")]
        [HarmonyPostfix]
        static void ScaleSuccessRate(ref double __result)
        {
            float mult = ModConfig.UpgradeSuccessRate.Value;
            if (mult.Approx(1.0f)) return;
            __result *= mult;
            if (__result > 1.0) __result = 1.0;
        }

        // ─── 取消强化等级上限 ────────────────────────────────────
        // Item.CanTeamSetUpgrade() → bool，原逻辑含 Level < 100
        // Postfix 重新判断：去掉 Level 上限，只要护身符 + 素材满足即可
        [HarmonyPatch(typeof(Item), "CanTeamSetUpgrade")]
        [HarmonyPostfix]
        static void RemoveUpgradeCap(Item __instance, ref bool __result)
        {
            if (!ModConfig.NoUpgradeCap.Value) return;
            if (__result) return; // 原方法已 true，不改
            __result = __instance.Type.GetResourceCategory() == ResourceCategory.Amulet
                       && __instance.TeamSetUpgradeRequirements().MetRequirements();
        }

        // ─── 打孔上限 ────────────────────────────────────────────
        // Item.CanAddMoreManualSockets() → bool
        // 原逻辑：武器 <4 孔，护甲 <3 孔；改为配置值
        [HarmonyPatch(typeof(Item), "CanAddMoreManualSockets")]
        [HarmonyPostfix]
        static void MaxSockets(Item __instance, ref bool __result)
        {
            int max = ModConfig.MaxSockets.Value;
            if (max <= 0) return;
            __result = __instance.Sockets.Count < max;
        }
    }
}