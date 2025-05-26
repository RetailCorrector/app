using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RetailCorrector.RegistryManager
{
    public readonly struct LocalModule(Assembly assembly, string path)
    {
        public Guid Id { get; } = new Guid(assembly.GetCustomAttribute<GuidAttribute>()!.Value);
        public string Name { get; } = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
        public Version Version { get; } = Version.Parse(assembly.GetCustomAttribute<AssemblyVersionAttribute>()!.Version);
        public string Hash { get; } = CalculateSHA512(File.ReadAllBytes(path));
        public string Path { get; } = path;

        private static string CalculateSHA512(byte[] data)
        {
            var hash = System.Security.Cryptography.SHA512.HashData(data);
            var buffer = new byte[88];
            System.Buffers.Text.Base64.EncodeToUtf8(hash, buffer, out _, out _);
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
    }
}
