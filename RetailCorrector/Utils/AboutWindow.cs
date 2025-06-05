using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace RetailCorrector.Utils;

public class AboutWindow : Window
{
    public AboutWindow()
    {
        var icon = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icon.ico", UriKind.Absolute)),
            Height = Width = 65,
            Margin = new Thickness(5),
            VerticalAlignment = VerticalAlignment.Top
        };
        var text = new TextBlock
        {
            Margin = new Thickness(0,5,15,5),
            TextWrapping = TextWrapping.WrapWithOverflow,
        };

        text.Inlines.Add(new Bold(new Run("RetailCorrector")));
        text.Inlines.Add(" - это программный комплекс, предназначенный для исправления ошибок в расчетах и регистрации ранее неучтённых операций на кассовом аппарате.");
        text.Inlines.Add(new LineBreak());
        text.Inlines.Add(new LineBreak());
        text.Inlines.Add(new Bold(new Run("RetailCorrector.Wizard")));
        text.Inlines.Add(" - утилита для подготовки чеков коррекции к отправке оператору фискальных документов. Программа выполняет парсинг, фильтрацию и редактирование чеков из указанного источника данных с целью исправления ранее отправленных чеков либо генерации отсутствующих чеков.");
        text.Inlines.Add(new LineBreak());
        text.Inlines.Add(new LineBreak());
        text.Inlines.Add("Версия: ");
        text.Inlines.Add(new Bold(new Run(App.Version)));
        text.Inlines.Add(new LineBreak());
        text.Inlines.Add("Разработчик: ");
        text.Inlines.Add(new Hyperlink(new Run("ornaras")));

        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle = WindowStyle.ToolWindow;
        ResizeMode = ResizeMode.CanMinimize;
        DockPanel.SetDock(icon, Dock.Left);
        var dock = new DockPanel();
        dock.Children.Add(icon);
        dock.Children.Add(text);
        Content = dock;
        Width = 350;
        Height = 325;
    }
}