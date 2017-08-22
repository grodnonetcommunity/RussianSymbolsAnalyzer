using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RussianSymbolsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RussianSymbolsAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RussianSymbolsAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(RussiaNamesAnalyzerAction, SyntaxKind.VariableDeclarator);
            context.RegisterSyntaxNodeAction(RussiaNamesParameterAnalyzerAction, SyntaxKind.Parameter);
        }

        private void RussiaNamesAnalyzerAction(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclaratorSyntax)context.Node;

            if (ContainsRussian(variableDeclaration.Identifier.ValueText))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, variableDeclaration.Identifier.GetLocation()));
            }
        }

        private void RussiaNamesParameterAnalyzerAction(SyntaxNodeAnalysisContext context)
        {
            var parameterSyntax = (ParameterSyntax)context.Node;

            if (ContainsRussian(parameterSyntax.Identifier.ValueText))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, parameterSyntax.Identifier.GetLocation()));
            }
        }

        private bool ContainsRussian(string identifierValueText)
        {
            return Regex.IsMatch(identifierValueText, "[а-я]", RegexOptions.IgnoreCase);
        }

        public void Test(int оссельхознадзор)
        {
            var оссельхознадзор1 = "asdasd";
        }

    }
}
