using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LVGLSharp.Analyzers;

/// <summary>
/// Ensures LVGLSharp.Forms consumers reference a platform runtime package.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RuntimePackageReferenceAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Usage";
    private const string FormsAssemblyName = "LVGLSharp.Forms";
    private const string WindowsRuntimeAssemblyName = "LVGLSharp.Runtime.Windows";
    private const string LinuxRuntimeAssemblyName = "LVGLSharp.Runtime.Linux";

    private static readonly LocalizableString MissingRuntimeTitle = new LocalizableResourceString("MissingRuntimeRuleTitle", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString MissingRuntimeMessage = new LocalizableResourceString("MissingRuntimeRuleMessage", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString MissingRuntimeDescription = new LocalizableResourceString("MissingRuntimeRuleDescription", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

    internal static readonly DiagnosticDescriptor MissingRuntimeRule = new(
        id: "LVGL003",
        title: MissingRuntimeTitle,
        messageFormat: MissingRuntimeMessage,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: MissingRuntimeDescription);

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [MissingRuntimeRule];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationAction(AnalyzeCompilation);
    }

    private static void AnalyzeCompilation(CompilationAnalysisContext context)
    {
        if (context.Compilation.AssemblyName is { } assemblyName && assemblyName.StartsWith("LVGLSharp.", StringComparison.Ordinal))
        {
            return;
        }

        var referencedAssemblies = context.Compilation.ReferencedAssemblyNames;
        if (!ContainsAssembly(referencedAssemblies, FormsAssemblyName))
        {
            return;
        }

        if (ContainsAssembly(referencedAssemblies, WindowsRuntimeAssemblyName) || ContainsAssembly(referencedAssemblies, LinuxRuntimeAssemblyName))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(MissingRuntimeRule, Location.None));
    }

    private static bool ContainsAssembly(IEnumerable<AssemblyIdentity> referencedAssemblies, string assemblyName)
    {
        foreach (var referencedAssembly in referencedAssemblies)
        {
            if (string.Equals(referencedAssembly.Name, assemblyName, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
