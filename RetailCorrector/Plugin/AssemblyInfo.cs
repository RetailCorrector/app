using System.Reflection;
using System.Runtime.InteropServices;
using RetailCorrector.Utils;

namespace RetailCorrector.Plugin;

public struct AssemblyInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Author { get; init; }
    public Version Version { get; init; }

    public AssemblyInfo(System.Reflection.Assembly assembly)
    {
        Id = new Guid(assembly.GetAttributeOrThrow<GuidAttribute>(Messages.MissingAssemblyId).Value);
        Name = $"{assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product}";
        Description = $"{assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? ""}";
        Author = $"{assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? ""}";
        Version = new Version(assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version);
    }

    public AssemblyInfo(AssemblyInfo src, string? id = null, string? name = null, string? desc = null, string? author = null, string? ver = null)
    {
        Id = id is null ? src.Id : new Guid(id);
        Name = name ?? src.Name;
        Description = desc ?? src.Description;
        Author = author ?? src.Author;
        Version = ver is null ? src.Version : new Version(ver);
    }
    
    public AssemblyInfo()
    {
        Id = Guid.Empty;
        Name = "";
        Description = "";
        Author = "";
        Version = new Version();
    }
}