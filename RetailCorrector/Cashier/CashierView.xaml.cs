using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using RetailCorrector.Plugin;
using RetailCorrector.Utils;
using System.ComponentModel;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.IO;

namespace RetailCorrector.Cashier{
    public partial class CashierView : Window, INotifyPropertyChanged
    {
        public int MinBuffer
        {
            get => _minBuffer;
            set
            {
                _minBuffer = value;
                OnPropertyChanged();
            }
        }
        private int _minBuffer = 20;
        public int MaxBuffer
        {
            get => _maxBuffer;
            set
            {
                _maxBuffer = value;
                OnPropertyChanged();
            }
        }
        private int _maxBuffer = 30;
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
        private int _progress = 0;
        public int MaxProgress
        {
            get => _maxProgress;
            set
            {
                _maxProgress = value;
                OnPropertyChanged();
            }
        }
        private int _maxProgress = 1;
        public FiscalPlugin? Plugin
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }
        private FiscalPlugin? _current;
        public ObservableCollection<UIElement> SettingsElements { get; } = [];

        private CancellationTokenSource _cancelSource = new();
        public bool Cancelled => _cancelSource.IsCancellationRequested;
        public bool Running => !_cancelSource.IsCancellationRequested;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "")=>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

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
                foreach (var receipt in Env.Receipts)
                {
                    if (await Plugin.CountDocsInBuffer() > MaxBuffer)
                    {
                        do { await Task.Delay(1000); }
                        while (await Plugin.CountDocsInBuffer() <= MinBuffer);
                    }
                    var res = await Plugin.ProcessingReceipt(receipt);
                    if (!res)
                        throw new Exception("Не удалось отбить чек! Подробнее в лог-файле...");
                    Progress++;
                }
            }
            catch (Exception ex)
            {
                await Plugin.Disconnect();
                AlertHelper.ErrorAlert(ex.Message);
                SetCancelling(true);
            }
        }
        
    }
}