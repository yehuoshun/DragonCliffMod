using HarmonyLib;

namespace DragonCliffMod.Patches
{
    /// <summary>
    /// 技能/英雄等级上限 patch。
    ///
    /// ⚠️ 待完成：英雄等级上限是 UnitExtensions.MaxAdventurerLevel（静态字段/常量），
    /// 但 UnitExtensions 类在 Assembly-CSharp-firstpass.dll 里（当前仓库只反编译了 Assembly-CSharp.dll）。
    /// 需反编译 firstpass 拿到 MaxAdventurerLevel 的定义（const 还是 static readonly）后，
    /// 决定用反射改字段还是 patch 读取点。参考引用点：
    ///   - AdventurerProfile.cs:356  `level + i + 1 > UnitExtensions.MaxAdventurerLevel`
    ///   - AdventurerProfile.cs:440  `this.GetLevel() < UnitExtensions.MaxAdventurerLevel`
    /// </summary>
    public static class SkillPatch
    {
#if false
        // 占位：等 firstpass 反编译后补真实 patch
        [HarmonyPatch(typeof(UnitExtensions), "SomeMethod")]
        [HarmonyPrefix]
        static bool Placeholder(ref int __result)
        {
            int cap = ModConfig.LevelCapOverride.Value;
            if (cap <= 0) return true;
            __result = cap;
            return false;
        }
#endif
    }
}