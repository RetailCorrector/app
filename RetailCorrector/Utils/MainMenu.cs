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
            Items.Add(Edit());
            Items.Add(Service());
            Items.Add(Help());
        }

        private static MenuItem Space() => new() 
        { 
            Header = "Пространство",
            ItemsSource = new MenuItem[]
            {
                NewItem("Создать новое", Commands.Clear),
                NewItem("Получить чеки", Commands.ParseReceipts),
                NewItem("Дизайнер отчета", Commands.OpenReportEditor),
            }
        };
        private static MenuItem Edit() => new() 
        { 
            Header = "Правка",
            ItemsSource = new Control[]
            {
                NewItem("Отменить", Commands.Undo),
                NewItem("Вернуть", Commands.Redo),
                new Separator(),
                NewItem("Добавить", Commands.AddReceipt),
                NewItem("Удалить", Commands.RemoveReceipts),
                NewItem("Дублировать", Commands.DuplicateReceipts),
                new Separator(),
                NewItem("Инвертировать выделение", Commands.InvertSelection),
                NewItem("Инвертировать тип чека", Commands.InvertOperation),
                new Separator(),
                NewItem("Массовые редактор", Commands.MultiEditor),
            }
        };
        private static MenuItem Service() => new() 
        { 
            Header = "Сервисы" ,
            ItemsSource = new Control[]
            {
                NewItem("Менеджер модулей", Commands.OpenPluginManager),
                NewItem("Локальный кассир", Commands.OpenCashier),
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