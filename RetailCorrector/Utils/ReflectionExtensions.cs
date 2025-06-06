using System.Reflection;

namespace RetailCorrector.Utils;

public static class ReflectionExtensions
{
    public static TAttr GetAttributeOrThrow<TAttr>(this Assembly assembly, string msg)
        where TAttr: Attribute
    {
        var attr = assembly.GetCustomAttribute<TAttr>();
        if (attr is null) throw new FormatException(msg);
        return attr;
    }
}