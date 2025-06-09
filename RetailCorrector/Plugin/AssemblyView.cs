using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RetailCorrector.Utils;
using Path = System.Windows.Shapes.Path;

namespace RetailCorrector.Plugin;

public class AssemblyView : UserControl, INotifyPropertyChanged
{
    public RemoteAssembly? Remote { get; init; }
    private Assembly? _local;
    private readonly AssemblyDownloader _parent;
    public bool IsEnabledRemove => _local is not null;
    public bool IsEnabledInstall => Remote is not null;
    public string LocalVersion => _local?.Info?.Version.ToString(3) ?? "-";

    private void Remove(object? s, RoutedEventArgs e)
    {
        var path = $"{_local!.Stream!.Name}";
        PluginController.Unload();
        File.Delete(path);
        PluginController.Load();
        _parent.UpdateModuleList();
    }

    private void Install(object? s, RoutedEventArgs e)
    {
        PluginController.Unload();
        using (var client = new HttpClient())
        {
            using var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, Remote!.Value.Download);
            using var resp = client.Send(req);
            using var content = resp.Content.ReadAsStream();
            var path = System.IO.Path.Combine(Pathes.Plugins, System.IO.Path.GetFileName(Remote!.Value.Download));
            using var file = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
            content.CopyTo(file);
        }
        PluginController.Load();
        _parent.UpdateModuleList();
    }

    public AssemblyView(RemoteAssembly origin, AssemblyDownloader parent)
    {
        _parent = parent;
        Remote = origin;
        UpdateLocalInfo();
        Draw();
    }
    public AssemblyView(Assembly local, AssemblyDownloader parent)
    {   
        _parent = parent;
        _local = local;
        Draw();
    }

    private void Draw()
    {
        ApplyStyles();
        ConfigureElement();
        var grid = GenerateParent();
        var title = new TextBlock 
        { 
            FontSize = 16, 
            FontWeight = FontWeights.Bold, 
            Text = Remote?.Info.Name ?? (_local?.Info?.Name ?? "") 
        };
        grid.AddChild(title, dx: 4);
        var desc = new TextBlock
        {
            TextWrapping = TextWrapping.WrapWithOverflow, 
            FontStyle = FontStyles.Italic, 
            Text = Remote?.Info.Description ?? (_local?.Info?.Description ?? "")
        };
        grid.AddChild(desc, dx: 4, y: 1);
        grid.AddChild(new TextBlock { Text = "Установлено" }, y: 3);
        grid.AddChild(new TextBlock { Text = "В реестре" }, x: 1, y: 3);
        var inst = new TextBlock { FontWeight = FontWeights.Bold, FontStyle = FontStyles.Italic };
        inst.SetBinding(TextBlock.TextProperty, nameof(LocalVersion));
        grid.AddChild(inst, y: 4);
        var reg = new TextBlock
        {
            FontWeight = FontWeights.Bold, 
            FontStyle = FontStyles.Italic,
            Text = Remote?.Info.Version.ToString(3) ?? "-"
        };
        grid.AddChild(reg, x: 1, y: 4);
        var bDel = new Button { Content = new Path { Data = Geometry.Parse(SvgImages.TrashBin) } };
        bDel.SetBinding(Button.IsEnabledProperty, nameof(IsEnabledRemove));
        bDel.Click += Remove;
        grid.AddChild(bDel, x: 2, y: 3, dy: 2);
        var bInst = new Button { Content = new Path { Data = Geometry.Parse(SvgImages.Install) } };
        bInst.Click += Install;
        bInst.SetBinding(Button.IsEnabledProperty, nameof(IsEnabledInstall));
        grid.AddChild(bInst, x: 4, y: 3, dy: 2);
        Content = grid;
    }

    private void UpdateLocalInfo()
    {
        _local = PluginController.Assemblies.FirstOrDefault(a => a.Info!.Value.Id == Remote!.Value.Info.Id);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocalVersion)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabledRemove)));
    }

    private void ConfigureElement()
    {
        Width = 422;
        MinHeight = 103;
        VerticalAlignment = VerticalAlignment.Top;
        BorderBrush = Brushes.Black;
        BorderThickness = new Thickness(1);
        Background = Brushes.White;
        DataContext = this;
    }

    private void ApplyStyles()
    {
        const string path = "pack://application:,,,/Plugin/AssemblyViewStyle.xaml";
        Resources = new ResourceDictionary { Source = new Uri(path, UriKind.Absolute) };
    }

    private static Grid GenerateParent()
    {
        var grid = new Grid { Margin = new Thickness(10) };
        
        grid.AddRow(new GridLength(21));
        grid.AddRow(new GridLength(1, GridUnitType.Star));
        grid.AddRow(new GridLength(9));
        grid.AddRow(new GridLength(16));
        grid.AddRow(new GridLength(16));
        
        grid.AddColumn(new GridLength(128));
        grid.AddColumn(new GridLength(1, GridUnitType.Star));
        grid.AddColumn(new GridLength(32));
        grid.AddColumn(new GridLength(5));
        grid.AddColumn(new GridLength(32));
        
        return grid;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}