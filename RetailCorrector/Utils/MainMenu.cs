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
            ItemsSource = new MenuItem[]
            {
                NewItem("Отменить", Commands.Undo),
                NewItem("Вернуть", Commands.Redo),
                NewItem("Инвертировать выделение", Commands.InvertSelection),
                NewItem("Добавить", Commands.AddReceipt),
                NewItem("Массовые редактор", Commands.MultiEditor),
                NewItem("Дублировать", Commands.DuplicateReceipts),
                NewItem("Инвертировать тип чека", Commands.InvertOperation),
                NewItem("Удалить", Commands.RemoveReceipts),
            }
        };
        private static MenuItem Service() => new() 
        { 
            Header = "Сервисы" ,
            ItemsSource = new MenuItem[]
            {
                NewItem("Менеджер модулей", Commands.OpenPluginManager),
                NewItem("Локальный кассир", Commands.OpenCashier)
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
            var item = new MenuItem { Header = text };
            item.SetBinding(MenuItem.CommandProperty, new Binding { Source = srcBinding });
            return item;
        }
    }
}