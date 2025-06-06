using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Utils;

public static class UIExtensions
{
    public static void AddChild(this Grid parent, UIElement child, 
        int x = 0, int y = 0, int dx = 1, int dy = 1)
    {
        if (x != 0) Grid.SetColumn(child, x);
        if (y != 0) Grid.SetRow(child, y);
        if (dx != 1) Grid.SetColumnSpan(child, dx);
        if (dy != 1) Grid.SetRowSpan(child, dy);
        parent.Children.Add(child);
    }

    public static void AddColumn(this Grid parent, GridLength width) =>
        parent.ColumnDefinitions.Add(new ColumnDefinition { Width = width });

    public static void AddRow(this Grid parent, GridLength height) =>
        parent.RowDefinitions.Add(new RowDefinition { Height = height });
}