using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RetailCorrector.Wizard.Extensions
{
    public static class EnumExtensions
    {
        public static KeyValuePair<TEnum, string>[] GetDisplayNames<TEnum>()
            where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            var list = new List<KeyValuePair<TEnum, string>>();
            for(var i = 0; i < values.Length; i++)
            {
                var value = values[i]!;
                var memb = typeof(TEnum).GetMember(value.ToString()!)[0];
                var attr = memb.GetCustomAttribute<DisplayAttribute>();
                if (attr is null) continue;
                list.Add(new KeyValuePair<TEnum, string>(value, attr!.Name!));
            }
            return [..list];
        }
        public static Array GetDisplayNames(Type type)
        {
            var values = Enum.GetValues(type);
            var list = new List<KeyValuePair<Enum, string>>();
            //var arr = Array.CreateInstance(typeof(KeyValuePair<Enum, string>), values.Length);
            for(var i = 0; i < values.Length; i++)
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
