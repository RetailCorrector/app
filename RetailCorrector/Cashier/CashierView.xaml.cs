using RetailCorrector.Plugins;
using RetailCorrector.Storage;
using RetailCorrector.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace RetailCorrector.Cashier{
    public partial class CashierView : Window
    {
        public ObservableCollection<UIElement> SettingsElements { get; } = [];

        private CancellationTokenSource _cancelSource = new();
        public bool Cancelled => _cancelSource.IsCancellationRequested;
        public bool Running => !_cancelSource.IsCancellationRequested;

        [NotifyUpdated] private int _minBuffer = 20;
        [NotifyUpdated] private int _maxBuffer = 30;
        [NotifyUpdated] private int _progress = 0;
        [NotifyUpdated] private int _maxProgress = 1;
        [NotifyUpdated] private FiscalPlugin? _plugin;

        private void SetCancelling(bool cancelling)
        {
            if(cancelling)
                _cancelSource.Cancel();
            else
                _cancelSource = new CancellationTokenSource();
            OnPropertyChanged(nameof(Cancelled));
            OnPropertyChanged(nameof(Running));
        }

        public CashierView()
        {
            _cancelSource.Cancel();
            PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(Plugin))
                    UpdateSettings();
            };

            CommandBindings.Add(new CommandBinding(Commands.ExitDialog, (_, _) => Close()));
            InitializeComponent();
        }

        private void UpdateSettings()
        {
            SettingsElements.Clear();
            if (Plugin is not null) 
            {
                var type = Plugin.GetType();
                var props = type.GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>() is not null).ToArray();
                for (var i = 0; i < props.Length; i++)
                {
                    var name = props[i].GetCustomAttribute<DisplayNameAttribute>()!.DisplayName;
                    var value = AutoSelectControl(props[i]);
                    value.Width = 168;
                    var lbl = new TextBlock
                    {
                        Text = name,
                        Height = value.Height,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 168,
                    };
                    UIExtensions.PositionInGrid(lbl, 0, i);
                    UIExtensions.PositionInGrid(value, 1, i);
                    SettingsElements.Add(lbl);
                    SettingsElements.Add(value);
                } 
            }
        }

        private Control AutoSelectControl(PropertyInfo info)
        {
            var binding = new Binding(info.Name) { Source = Plugin, Mode = BindingMode.TwoWay };
            Control Bind(Control control, DependencyProperty dp)
            {
                control.SetBinding(dp, binding);
                return control;
            }

            var type = info.PropertyType;
            if (type.IsEnum)
            {
                var combobox = new ComboBox
                {
                    ItemsSource = EnumHelper.GetDisplayNames(type),
                    SelectedValuePath = "Key",
                    DisplayMemberPath = "Value",
                    Name = info.Name
                };
                return Bind(combobox, ComboBox.SelectedValueProperty);
            }
            if(type == typeof(bool))
            {
                var combobox = new ComboBox
                {
                    ItemsSource = new KeyValuePair<bool, string>[]
                    {
                        new(true, "Да"),
                        new(false, "Нет")
                    },
                    SelectedValuePath = "Key",
                    DisplayMemberPath = "Value",
                    Name = info.Name
                };
                return Bind(combobox, ComboBox.SelectedValueProperty);
            }
            return type switch
            {
                _ when type == typeof(string) => Bind(new TextBox(), TextBox.TextProperty),
                _ when type == typeof(double) => Bind(new DoubleUpDown(), DoubleUpDown.ValueProperty),
                _ when type == typeof(float) => Bind(new SingleUpDown(), SingleUpDown.ValueProperty),
                _ when type == typeof(decimal) => Bind(new DecimalUpDown(), DecimalUpDown.ValueProperty),
                _ when type == typeof(long) => Bind(new LongUpDown(), LongUpDown.ValueProperty),
                _ when type == typeof(ulong) => Bind(new ULongUpDown(), ULongUpDown.ValueProperty),
                _ when type == typeof(short) => Bind(new ShortUpDown(), ShortUpDown.ValueProperty),
                _ when type == typeof(ushort) => Bind(new UShortUpDown(), UShortUpDown.ValueProperty),
                _ when type == typeof(byte) => Bind(new ByteUpDown(), ByteUpDown.ValueProperty),
                _ when type == typeof(sbyte) => Bind(new SByteUpDown(), SByteUpDown.ValueProperty),
                _ when type == typeof(int) => Bind(new IntegerUpDown(), IntegerUpDown.ValueProperty),
                _ when type == typeof(uint) => Bind(new UIntegerUpDown(), UIntegerUpDown.ValueProperty),
                _ => throw new NotSupportedException($"Тип {type.Name} не поддерживается")
            };
        }


        private void Cancel(object? s, RoutedEventArgs? e) => SetCancelling(true);
        private async void Start(object? s, RoutedEventArgs? e)
        {
            if(Plugin is null)
            {
                System.Windows.MessageBox.Show("Выберите модуль для работы с чеками!");
                return;
            }
            SetCancelling(false);
            try
            {
                if (!await Plugin.Connect())
                    throw new Exception("Не удалось подключиться...");
                foreach (var receipt in StorageContext.Instance.Receipts)
                {
                    if (await Plugin.CountDocsInBuffer() > MaxBuffer)
                    {
                        do { await Task.Delay(1000); }
                        while (await Plugin.CountDocsInBuffer() <= MinBuffer);
                    }
                    var res = await Plugin.ProcessingReceipt(receipt);
                    if (!res) throw new Exception();
                    Progress++;
                }
            }
            catch (Exception ex)
            {
                await Plugin.Disconnect();
            }
            finally
            {
                SetCancelling(true);
            }
        }
        
    }
}