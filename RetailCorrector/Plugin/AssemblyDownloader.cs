using Masonry;
using RetailCorrector.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace RetailCorrector.Plugin;

public class AssemblyDownloader : Window, INotifyPropertyChanged
{
    public ObservableCollection<AssemblyView> Items { get; } = [];
    
    private string _currentRegistry;
    public string CurrentRegistry
    {
        get => _currentRegistry;
        set
        {
            if (_currentRegistry == value) return;
            _currentRegistry = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRegistry)));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private Command ShowRegistries { get; } = new(_ => new RegistryList().ShowDialog());
    
    public AssemblyDownloader()
    {
        PropertyChanged += (_, e) =>
        {
            if(e.PropertyName == nameof(CurrentRegistry)) UpdateModuleList();
        };
        CommandBindings.Add(new CommandBinding(Commands.ExitDialog, (_, _) => Close()));
        CurrentRegistry = RegistryList.Registries.GetValueOrDefault(0, Links.DefaultRegistry);
        Draw();
    }
    
    private void UpdateModuleList()
    {
        Items.Clear();
        var remote = PullRemote().Result;
        foreach (var assembly in remote.Assemblies)
            Items.Add(new AssemblyView(assembly, this));
        foreach (var local in PluginController.Assemblies)
        {
            if(!remote.Assemblies.Any(a => a.Info.Id == ((AssemblyInfo)local.Info!).Id))
                Items.Add(new AssemblyView(local, this));
        }
    }

    private void Refresh(object? s, RoutedEventArgs e) => UpdateModuleList();

    private async Task<RemoteAssemblies> PullRemote()
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var uri = new Uri(CurrentRegistry, UriKind.Absolute);
            using var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, uri);
            req.Headers.Add("User-Agent", $"RetailCorrector/{App.Version}");
            using var resp = await client.SendAsync(req).ConfigureAwait(false);
            var content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<RemoteAssemblies>(content);
        }
        catch
        {
            return new RemoteAssemblies();
        }
    }

    #region UI

    private static Grid GenerateParent()
    {
        var grid = new Grid { Margin = new Thickness(0, 10, 0, 10) };
        
        grid.AddColumn(new GridLength(10));
        grid.AddColumn(new GridLength(1, GridUnitType.Star));
        grid.AddColumn(new GridLength(5));
        grid.AddColumn(new GridLength(22));
        grid.AddColumn(new GridLength(5));
        grid.AddColumn(new GridLength(22));
        grid.AddColumn(new GridLength(10));
        
        grid.AddRow(new GridLength(22));
        grid.AddRow(new GridLength(5));
        grid.AddRow(new GridLength(1, GridUnitType.Star));
        
        return grid;
    }
    private void ConfigureWindow()
    {
        ResizeMode = ResizeMode.NoResize;
        DataContext = this;
        Height = 450;
        Width = 885;
        Title = "Менеджер модулей";
    }
    private void Draw()
    {
        ConfigureWindow();
        var grid = GenerateParent();
        var registries = new WatermarkComboBox { Watermark = "Реестры плагинов не найдены" };
        registries.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = RegistryList.Registries });
        registries.SetBinding(ComboBox.SelectedItemProperty, nameof(CurrentRegistry));
        grid.AddChild(registries, 1);
        var refresh = new Button { 
            Content = "",
            FontFamily = new FontFamily("Segoe MDL2 Assets")
        };
        refresh.Click += Refresh;
        grid.AddChild(refresh, 3);
        grid.AddChild(new Button { Content = "...", Command = ShowRegistries }, 5);
        var masonry = new MasonryControl { Spacing = 5 };
        masonry.SetBinding(ItemsControl.ItemsSourceProperty, nameof(Items));
        var scroll = new ScrollViewer
        {
            VerticalScrollBarVisibility=ScrollBarVisibility.Hidden,
            HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled,
            Content = masonry
        };
        grid.AddChild(scroll, 1, 2, 6);
        Content = grid;
    }

    #endregion
}