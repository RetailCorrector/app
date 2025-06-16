using RetailCorrector.History;
using RetailCorrector.History.Actions;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RetailCorrector.Editor.Receipt
{
    public partial class ReceiptView : FrameworkElement
    {
        public readonly static DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(RetailCorrector.Receipt), typeof(ReceiptView));

        [NotifyUpdated] private bool _isSelected = false;

        public RetailCorrector.Receipt DataSource
        {
            get => (RetailCorrector.Receipt)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == nameof(DataSource))
                InvalidateVisual();
        }

        public ReceiptView()
        {
            PropertyChanged += (_, _) => InvalidateVisual();
            Width = 190;
            MouseLeftButtonDown += SwitchSelection;
            MouseRightButtonDown += OpenDialog;
            VerticalAlignment = VerticalAlignment.Top;
        }

        protected override void OnRender(DrawingContext ctx)
        {
            var pen = new Pen(Brushes.Black, IsSelected ? 2 : 1);
            var operation = DisplayInfo.Operations[DataSource.Operation];
            if (!operation.EndsWith('а')) operation += 'а';
            var text = GenText($"Чек {operation.ToLower()}", 13);
            ctx.DrawText(text, new Point((190 - text.Width) / 2, 5));
            text = GenText(DisplayInfo.CorrectionTypes[DataSource.CorrectionType]);
            ctx.DrawText(text, new Point(5, 21));
            text = GenText(DataSource.ActNumber ?? "");
            ctx.DrawText(text, new Point(185 - text.Width, 21));
            text = GenText("---------------------------");
            ctx.DrawText(text, new Point(5, 32));

            var y = 43.0;
            foreach (var pos in DataSource.Items)
                AddPosition(ctx, pos, ref y);


            text = GenText("ИТОГ", 14, true);
            ctx.DrawText(text, new Point(5, y));
            text = GenText($"≡{(DataSource.TotalSum/100.0):F2}", 14, true);
            ctx.DrawText(text, new Point(185 - text.Width, y));
            y += 14;

            AddPayment(ctx, "НАЛИЧНЫЕ", DataSource.Payment.Cash, ref y);
            AddPayment(ctx, "БЕЗНАЛИЧНЫЕ", DataSource.Payment.ECash, ref y);
            AddPayment(ctx, "АВАНС", DataSource.Payment.Pre, ref y);
            AddPayment(ctx, "КРЕДИТ", DataSource.Payment.Post, ref y);
            AddPayment(ctx, "ДРУГОЕ", DataSource.Payment.Provision, ref y);
            y += 7;

            text = GenText($"{DataSource.Created:yyyy'-'MM'-'dd}");
            ctx.DrawText(text, new Point(5, y));
            text = GenText($"{DataSource.FiscalSign}");
            ctx.DrawText(text, new Point(185 - text.Width, y));

            y += 11;
            Height = y + 10;
            var down = Height;
            ctx.DrawLine(pen, new Point(190, -4), new Point(190, down));
            ctx.DrawLine(pen, new Point(0, -4), new Point(0, down));
            for (var i = 0; i < 38; i++)
            {
                if (i % 2 == 0)
                {
                    ctx.DrawLine(pen, new Point(i * 5, -4), new Point((i + 1) * 5, 0)); // down
                    ctx.DrawLine(pen, new Point(i * 5, down), new Point((i + 1) * 5, down - 4));// up
                }
                else
                {
                    ctx.DrawLine(pen, new Point(i * 5, 0), new Point((i + 1) * 5, -4));// up
                    ctx.DrawLine(pen, new Point(i * 5, down - 4), new Point((i + 1) * 5, down));// down
                }
            }
            ctx.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Transparent, 0), new Rect(0, 0, 190, Height));
            base.OnRender(ctx);
        }

        private static FormattedText GenText(string text, int size = 11, bool bold = false) =>
            new(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, 
                    bold ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal),
                size, Brushes.Black, 1)
            {
                MaxTextWidth = 180
            };

        private static void AddPosition(DrawingContext ctx, Position pos, ref double y)
        {
            var text = GenText(pos.Name);
            ctx.DrawText(text, new Point(5, y));
            y += text.Height;
            var unit = DisplayInfo.ShortMeasureUnits.GetValueOrDefault(pos.MeasureUnit, "");
            text = GenText($"{(pos.Price / 100.0):F2} x {(pos.Quantity / 1000.0):0.###} {unit}");
            ctx.DrawText(text, new Point(15, y));
            text = GenText($"{(pos.TotalSum / 100.0):F2}");
            ctx.DrawText(text, new Point(185 - text.Width, y));
            y += text.Height;
            text = GenText("---------------------------");
            ctx.DrawText(text, new Point(5, y));
            y += text.Height;
        }

        private static void AddPayment(DrawingContext ctx, string name, uint value, ref double y)
        {
            if (value == 0) return;
            var text = GenText(name);
            ctx.DrawText(text,new Point(10, y));
            text = GenText($"≡{(value/100.0):F2}");
            ctx.DrawText(text,new Point(185-text.Width, y));
            y += text.Height;
        }

        public void SwitchSelection(object? s, RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
            Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void OpenDialog(object sender, MouseButtonEventArgs e)
        {
            var i = Env.Receipts.IndexOf(DataSource);
            var wizard = new ReceiptWizard(DataSource);
            if(wizard.ShowDialog() == true)
                HistoryController.Add(new EditReceipts(i, wizard.Data));
        }
    }
}
