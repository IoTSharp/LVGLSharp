using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LVGLSharp.Analyzers;

/// <summary>
/// Enforces the demo layout pattern that LVGL currently renders reliably.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DemoLayoutAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Layout";
    private const string TableLayoutPanelMetadataName = "System.Windows.Forms.TableLayoutPanel";
    private const string LvglTableLayoutPanelMetadataName = "LVGLSharp.Forms.TableLayoutPanel";
    private const string FlowLayoutPanelMetadataName = "System.Windows.Forms.FlowLayoutPanel";
    private const string LvglFlowLayoutPanelMetadataName = "LVGLSharp.Forms.FlowLayoutPanel";
    private const string SizeTypeMetadataName = "System.Windows.Forms.SizeType";
    private const string LvglSizeTypeMetadataName = "LVGLSharp.Forms.SizeType";

    private static readonly LocalizableString DirectChildTitle = new LocalizableResourceString("DirectChildRuleTitle", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString DirectChildMessage = new LocalizableResourceString("DirectChildRuleMessage", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString DirectChildDescription = new LocalizableResourceString("DirectChildRuleDescription", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString AbsoluteRowTitle = new LocalizableResourceString("AbsoluteRowRuleTitle", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString AbsoluteRowMessage = new LocalizableResourceString("AbsoluteRowRuleMessage", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString AbsoluteRowDescription = new LocalizableResourceString("AbsoluteRowRuleDescription", AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

    internal static readonly DiagnosticDescriptor DirectChildRule = new(
        id: "LVGL001",
        title: DirectChildTitle,
        messageFormat: DirectChildMessage,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DirectChildDescription);

    internal static readonly DiagnosticDescriptor AbsoluteRowRule = new(
        id: "LVGL002",
        title: AbsoluteRowTitle,
        messageFormat: AbsoluteRowMessage,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: AbsoluteRowDescription);

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [DirectChildRule, AbsoluteRowRule];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        if (!IsDemoDesignerFile(context.Node.SyntaxTree))
        {
            return;
        }

        if (context.Node is not InvocationExpressionSyntax invocation || invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        if (memberAccess.Name.Identifier.ValueText != "Add" || memberAccess.Expression is not MemberAccessExpressionSyntax collectionAccess)
        {
            return;
        }

        switch (collectionAccess.Name.Identifier.ValueText)
        {
            case "Controls":
                AnalyzeControlsAdd(context, invocation, collectionAccess);
                break;

            case "RowStyles":
                AnalyzeRowStylesAdd(context, invocation, collectionAccess);
                break;
        }
    }

    private static void AnalyzeControlsAdd(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation, MemberAccessExpressionSyntax collectionAccess)
    {
        if (!IsTableLayoutPanel(context.SemanticModel.GetTypeInfo(collectionAccess.Expression, context.CancellationToken).Type))
        {
            return;
        }

        if (invocation.ArgumentList.Arguments.Count == 0)
        {
            return;
        }

        var childExpression = invocation.ArgumentList.Arguments[0].Expression;
        var childType = context.SemanticModel.GetTypeInfo(childExpression, context.CancellationToken).Type;
        if (IsFlowLayoutPanel(childType))
        {
            return;
        }

        var childTypeName = childType?.Name ?? childExpression.ToString();
        context.ReportDiagnostic(Diagnostic.Create(DirectChildRule, childExpression.GetLocation(), childTypeName));
    }

    private static void AnalyzeRowStylesAdd(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation, MemberAccessExpressionSyntax collectionAccess)
    {
        if (!IsTableLayoutPanel(context.SemanticModel.GetTypeInfo(collectionAccess.Expression, context.CancellationToken).Type))
        {
            return;
        }

        if (invocation.ArgumentList.Arguments.Count == 0)
        {
            return;
        }

        if (invocation.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax objectCreation || objectCreation.ArgumentList is null)
        {
            return;
        }

        if (objectCreation.ArgumentList.Arguments.Count == 0)
        {
            return;
        }

        var sizeTypeExpression = objectCreation.ArgumentList.Arguments[0].Expression;
        if (!IsPercentSizeType(context.SemanticModel.GetSymbolInfo(sizeTypeExpression, context.CancellationToken).Symbol))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(AbsoluteRowRule, sizeTypeExpression.GetLocation()));
    }

    private static bool IsDemoDesignerFile(SyntaxTree syntaxTree)
    {
        var filePath = syntaxTree.FilePath;
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        var normalizedPath = filePath.Replace('\\', '/');
        return normalizedPath.Contains("/src/Demos/", StringComparison.OrdinalIgnoreCase)
            && normalizedPath.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTableLayoutPanel(ITypeSymbol? typeSymbol)
        => IsNamedType(typeSymbol, TableLayoutPanelMetadataName) || IsNamedType(typeSymbol, LvglTableLayoutPanelMetadataName);

    private static bool IsFlowLayoutPanel(ITypeSymbol? typeSymbol)
        => IsNamedType(typeSymbol, FlowLayoutPanelMetadataName) || IsNamedType(typeSymbol, LvglFlowLayoutPanelMetadataName);

    private static bool IsPercentSizeType(ISymbol? symbol)
        => symbol is IFieldSymbol fieldSymbol
            && fieldSymbol.Name == "Percent"
            && (fieldSymbol.ContainingType.ToDisplayString() == SizeTypeMetadataName || fieldSymbol.ContainingType.ToDisplayString() == LvglSizeTypeMetadataName);

    private static bool IsNamedType(ITypeSymbol? typeSymbol, string metadataName)
    {
        for (var current = typeSymbol; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == metadataName)
            {
                return true;
            }
        }

        return false;
    }
}
