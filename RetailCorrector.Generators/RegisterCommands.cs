using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;

namespace RetailCorrector.Generators
{
    [Generator]
    public class RegisterCommands : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var q = context.SyntaxProvider.CreateSyntaxProvider(
                (node, _) => node is ClassDeclarationSyntax,
                (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol(ctx.Node));
            context.RegisterSourceOutput(q, Execute);
        }

        private void Execute(SourceProductionContext ctx, ISymbol source)
        {
            var builder = new StringBuilder();
            if (source.ToDisplayString() != "RetailCorrector.Constants.HotKeys")
                return;
            var type = (ITypeSymbol)source;
            var members = type.GetMembers().Where(m => m is IFieldSymbol).ToArray();
            builder.AppendLine(@$"using System.Windows.Input;

namespace RetailCorrector.Utils;

public static partial class Commands
{{");
            foreach (var prop in members)
                builder.AppendLine($"    public static RoutedCommand {prop.Name} {{ get; }} = new(nameof({prop.Name}), typeof(Commands));");
            builder.AppendLine(@"
    private static void SetupHotKeys()
    {");
            foreach (var prop in members)
                builder.AppendLine($"        {prop.Name}.InputGestures.Add(HotKeys.{prop.Name});");
            builder.AppendLine(@"    }
}");

            //public static RoutedCommand Undo { get; } = new(nameof(Undo), typeof(Commands));
            //Undo.InputGestures.Add(HotKeys.Undo);

            ctx.AddSource("Commands.g.cs", builder.ToString());
        }
    }
}
