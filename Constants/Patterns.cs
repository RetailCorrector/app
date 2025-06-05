using System.Text.RegularExpressions;

namespace RetailCorrector.Constants;

public static partial class Patterns
{
    public const string OutputLog = "{Timestamp:yyyy-MM-ss HH:mm:ss.fff zzz} [{Level:u3}] ({Version}) {Message:lj}{NewLine}{Exception}";

    [GeneratedRegex(@"^(?'name'[a-zA-Z _-]+) \((?'guid'[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12})\)$")]
    public static partial Regex AssemblyNameRegex();
}
