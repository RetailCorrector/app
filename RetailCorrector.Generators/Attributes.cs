using Microsoft.CodeAnalysis;
using System.Text;

namespace RetailCorrector.Generators
{
    [Generator]
    public class Attributes : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                NotifyUpdatedAttribute(ctx);
            });
        }

        private void NotifyUpdatedAttribute(IncrementalGeneratorPostInitializationContext ctx)
        {
            var builder = new StringBuilder();
            builder.AppendLine("namespace RetailCorrector.Attributes;");
            builder.AppendLine();
            builder.AppendLine("[AttributeUsage(AttributeTargets.Field)]");
            builder.AppendLine("public class NotifyUpdatedAttribute(params string[] notifyProperties) : Attribute;");
            ctx.AddSource("NotifyUpdatedAttribute.g.cs", builder.ToString());
        }
    }
}
