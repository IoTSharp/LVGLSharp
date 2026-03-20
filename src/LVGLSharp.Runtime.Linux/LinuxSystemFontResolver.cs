using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.Fonts;
using SixLabors.Fonts.Unicode;

namespace LVGLSharp.Runtime.Linux;

internal static class LinuxSystemFontResolver
{
    private static readonly Lazy<FontFamily?> s_cachedFontFamily = new(TryResolveFontFamilyCore);

    private static readonly string[] s_preferredFamilyNames =
    [
        "Noto Sans CJK SC",
        "Noto Serif CJK SC",
        "Source Han Sans SC",
        "Source Han Sans CN",
        "WenQuanYi Zen Hei",
        "Droid Sans Fallback",
        "AR PL UMing CN",
        "AR PL UKai CN",
        "Microsoft YaHei",
        "SimHei",
        "Noto Sans Mono CJK SC",
        "Noto Sans CJK JP",
        "DejaVu Sans",
        "Liberation Sans",
        "FreeSans",
    ];

    private static readonly string s_fontCoverageSample =
        "PictureBox演示程序图像路径输入加载显示模式抗锯齿左旋右旋放大缩小重置就绪串口刷新波特率打开关闭发送接收文本按钮菜单设备文件路径";

    internal static FontFamily? TryResolveFontFamily()
    {
        return s_cachedFontFamily.Value;
    }

    private static FontFamily? TryResolveFontFamilyCore()
    {
        HashSet<string> seenNames = new(StringComparer.OrdinalIgnoreCase);
        List<FontFamily> preferredFamilies = new();

        foreach (var preferredFamilyName in s_preferredFamilyNames)
        {
            if (SystemFonts.TryGet(preferredFamilyName, out var preferredFamily) && seenNames.Add(preferredFamily.Name))
            {
                preferredFamilies.Add(preferredFamily);
            }
        }

        var preferredMatch = TryResolveBestFamily(preferredFamilies);
        if (preferredMatch is not null)
        {
            return preferredMatch;
        }

        List<FontFamily> fallbackFamilies = new();
        foreach (var family in SystemFonts.Families)
        {
            if (seenNames.Add(family.Name))
            {
                fallbackFamilies.Add(family);
            }
        }

        return TryResolveBestFamily(fallbackFamilies);
    }

    private static FontFamily? TryResolveBestFamily(IEnumerable<FontFamily> candidateFamilies)
    {
        FontFamily? bestFamily = null;
        FontCandidateScore bestScore = default;

        foreach (var family in candidateFamilies)
        {
            if (!TryScoreFamily(family, out var score))
            {
                continue;
            }

            if (bestFamily is null || score.CompareTo(bestScore) > 0)
            {
                bestFamily = family;
                bestScore = score;
            }
        }

        return bestScore.Coverage > 0 ? bestFamily : null;
    }

    private static bool TryScoreFamily(FontFamily family, out FontCandidateScore score)
    {
        score = default;

        try
        {
            var font = family.CreateFont(12);
            int glyphCoverage = 0;

            foreach (var rune in s_fontCoverageSample.EnumerateRunes())
            {
                if (Rune.IsWhiteSpace(rune))
                {
                    continue;
                }

                if (font.TryGetGlyphs(new CodePoint(rune.Value), out var glyphs) && glyphs.Count > 0)
                {
                    glyphCoverage++;
                }
            }

            int preferredRank = Array.FindIndex(
                s_preferredFamilyNames,
                preferred => string.Equals(preferred, family.Name, StringComparison.OrdinalIgnoreCase));

            score = new FontCandidateScore(
                glyphCoverage,
                preferredRank >= 0 ? s_preferredFamilyNames.Length - preferredRank : 0,
                IsSansFamily(family.Name) ? 1 : 0,
                string.Equals(family.Culture?.TwoLetterISOLanguageName, "zh", StringComparison.OrdinalIgnoreCase) ? 1 : 0);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsSansFamily(string familyName)
    {
        return familyName.Contains("Sans", StringComparison.OrdinalIgnoreCase) ||
               familyName.Contains("Hei", StringComparison.OrdinalIgnoreCase);
    }

    private readonly record struct FontCandidateScore(
        int Coverage,
        int PreferredRank,
        int SansBonus,
        int ChineseCultureBonus)
        : IComparable<FontCandidateScore>
    {
        public int CompareTo(FontCandidateScore other)
        {
            int coverageComparison = Coverage.CompareTo(other.Coverage);
            if (coverageComparison != 0)
            {
                return coverageComparison;
            }

            int preferredComparison = PreferredRank.CompareTo(other.PreferredRank);
            if (preferredComparison != 0)
            {
                return preferredComparison;
            }

            int sansComparison = SansBonus.CompareTo(other.SansBonus);
            if (sansComparison != 0)
            {
                return sansComparison;
            }

            return ChineseCultureBonus.CompareTo(other.ChineseCultureBonus);
        }
    }
}
