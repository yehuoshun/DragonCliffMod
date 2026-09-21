using System.Collections.Generic;
using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 酒馆招募冒险者相关 patch。
    /// </summary>
    public static class AdventurerPatch
    {
        // ─── 必出星辰冒险者 ────────────────────────────────────
        // RecruitmentFacility.GenerateProfiles(int, double starChance)
        // 原逻辑：starChance 只有 0（未解锁2星）或 0.1（已解锁）
        [HarmonyPatch(typeof(RecruitmentFacility), "GenerateProfiles")]
        [HarmonyPrefix]
        static void ForceStarAdventurer(ref double starChance)
        {
            if (!ModConfig.ForceStarAdventurer.Value) return;
            starChance = 1.0;
        }

        // ─── 满天赋（天赋点满 17 点）───────────────────────────
        // 生成候选人后，把每个 Profile 的 TalentPoints 设为 17
        [HarmonyPatch(typeof(RecruitmentFacility), "GenerateProfiles")]
        [HarmonyPostfix]
        static void FullTalent(ref List<AdventurerCandidate> __result)
        {
            if (!ModConfig.FullTalentAdventurer.Value) return;
            foreach (AdventurerCandidate c in __result)
            {
                if (c.Profile != null)
                {
                    c.Profile.TalentPoints = 17;
                }
            }
        }
    }
}