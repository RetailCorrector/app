using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RetailCorrector.Utils
{
    public static class EnumHelper
    {
        public static Array GetDisplayNames(Type type)
        {
            var values = Enum.GetValues(type);
            var list = new List<KeyValuePair<Enum, string>>();
            for (var i = 0; i < values.Length; i++)
            {
                var value = values.GetValue(i)!;
                var memb = type.GetMember(value.ToString()!)[0];
                var attr = memb.GetCustomAttribute<DisplayAttribute>();
                if (attr is null) continue;
                list.Add(new KeyValuePair<Enum, string>((Enum)value, attr!.Name!));
            }
            return list.ToArray();
        }
    }
}