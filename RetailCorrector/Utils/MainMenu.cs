using System.Windows.Controls;
using System.Windows.Data;

namespace RetailCorrector.Utils
{
    public class MainMenu : Menu
    {
        public MainMenu()
        {
            IsMainMenu = true;
            DockPanel.SetDock(this, Dock.Top);
            Items.Add(Space());
            Items.Add(Service());
            Items.Add(Help());
        }

        private static MenuItem Space() => new() 
        { 
            Header = "Пространство",
            ItemsSource = new Control[]
            {
                NewItem("Создать новое", Commands.Clear),
                new Separator(),
                NewItem("Парсер чеков", Commands.ParseReceipts),
                NewItem("Добавить чек", Commands.AddReceipt),
                new Separator(),
                NewItem("Дизайнер отчета", Commands.OpenReportEditor),
                new Separator(),
                NewItem("Подготовка локального агента", Commands.LocalExport),
            }
        };
        private static MenuItem Service() => new() 
        { 
            Header = "Сервисы" ,
            ItemsSource = new Control[]
            {
                NewItem("Менеджер модулей", Commands.OpenPluginManager),
                new Separator(),
            }
        };
        private static MenuItem Help() => new()
        {
            Header = "Помощь",
            ItemsSource = new MenuItem[] {
                NewItem("Открыть журнал сессии", Commands.OpenConsole),
                NewItem("Открыть лог-папку", Commands.OpenLogDir),
                NewItem("Документация", Commands.OpenDocs),
                NewItem("О программе", Commands.OpenAbout)
            }
        };

        private static MenuItem NewItem(string text, object srcBinding)
        {
            var item = new MenuItem { 
                Header = text, 
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left,  
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center
            };
            item.SetBinding(MenuItem.CommandProperty, new Binding { Source = srcBinding });
            return item;
        }
    }
}