using RetailCorrector.Wizard.Repository;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard
{
    public partial class ModuleInfo : UserControl, INotifyPropertyChanged
    {
        public enum ModuleStatus { NotInstalled, Installed, Newer, Broken, Older }
        public RepoPackage Package => _package;
        private RepoPackage _package;

        public ModuleStatus Status
        {
            get
            {
                if(ModuleCollection.Modules.FirstOrDefault(m => m.Guid == _package.Guid) is null) 
                    return ModuleStatus.NotInstalled;
                if(ModuleCollection.Modules.First(m => m.Guid == _package.Guid).Version < _package.Version)
                    return ModuleStatus.Newer;
                if(ModuleCollection.Modules.First(m => m.Guid == _package.Guid).Version > _package.Version)
                    return ModuleStatus.Older;
                if(ModuleCollection.Modules.First(m => m.Guid == _package.Guid).HashSum != _package.HashSum)
                    return ModuleStatus.Broken;
                return ModuleStatus.Installed;
            }
        }

        public string InstallText => Status switch
        {
            ModuleStatus.NotInstalled => "Установить",
            ModuleStatus.Installed => "Удалить",
            ModuleStatus.Newer => "Обновить",
            ModuleStatus.Older => "Откад",
            ModuleStatus.Broken => "Переустановить",
            _ => "???"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string nameProperty) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));

        public ModuleInfo(RepoPackage package)
        {
            _package = package;
            InitializeComponent();
        }

        private async void Install(object? sender, RoutedEventArgs args)
        {
            var tempStatus = Status;
            if (tempStatus >= ModuleStatus.Installed)
            {
                var _path = ModuleCollection.Remove(_package.Guid);
                File.Delete(_path);
            }
            if (tempStatus != ModuleStatus.Installed)
            {
            using var http = new HttpClient();
            using var resp = await http.GetAsync(_package.Uri);
            using var stream = await resp.Content.ReadAsStreamAsync();
            var path = Path.Combine(AppContext.BaseDirectory, 
                "sources", _package.Uri.ToString().Split('/')[^1]);
            using (var fs = File.Create(path))
                stream.CopyTo(fs);
            await ModuleCollection.Add(path);
            }
            OnPropertyChanged(nameof(InstallText));
        }
    }
}
