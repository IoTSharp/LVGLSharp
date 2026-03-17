using System.Resources;

namespace LVGLSharp.Analyzers;

internal static class AnalyzerResources
{
    private static readonly ResourceManager s_resourceManager = new("LVGLSharp.Analyzers.AnalyzerResources", typeof(AnalyzerResources).Assembly);

    internal static ResourceManager ResourceManager => s_resourceManager;
}
