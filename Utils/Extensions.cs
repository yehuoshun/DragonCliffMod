using System.Runtime.CompilerServices;

namespace DragonCliffMod.Utils;

/// <summary>
/// 工具扩展方法。
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 近似相等比较（处理 float 精度）。
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approx(this float a, float b, float epsilon = 1e-6f)
        => Math.Abs(a - b) < epsilon;
}