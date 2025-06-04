using RetailCorrector.Wizard.Converters;
using RetailCorrector.Wizard.HistoryActions;
using RetailCorrector.Wizard.Managers;
using RetailCorrector.Wizard.ModuleSystem;
using RetailCorrector.Wizard.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace RetailCorrector.Wizard.Windows
{
    public partial class Parser : Window, INotifyPropertyChanged
    {
        private AbstractSourceModule? _module;
        public AbstractSourceModule? Module
        {
            get => _module;
            set
            {
                _module = value;
                OnPropertyChanged();
            }
        }

        private int _maxProgress = 1;
        public int MaxProgress
        {
            get => _maxProgress;
            set
            {
                _maxProgress = value;
                OnPropertyChanged();
            }
        }

        private int _currProgress = 0;
        public int CurrProgress
        {
            get => _currProgress;
            set
            {
                _currProgress = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabledCancelButton => !CancelSource.IsCancellationRequested;
        public bool IsEnabledStartButton => CancelSource.IsCancellationRequested;

        public ObservableCollection<UIElement> Items { get; } = [];
        public CancellationTokenSource CancelSource { get; set; } = new();

        public Parser()
        {
            CancelSource.Cancel();
            ModuleCollection.Load().Wait();
            PropertyChanged += ModuleChanged;
            InitializeComponent();
        }

        private void ModuleChanged(object? sender, PropertyChangedEventArgs e)
        {
            Items.Clear();
            if (Module is null) return;
            foreach(var prop in Module.GetType().GetProperties())
            {
                var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (attr is null) continue;
                var control = new OptionControl(attr.DisplayName);
                var name = prop.PropertyType.FullName;
                FrameworkElement item = null!;
                var bind = new Binding(prop.Name) { Source = Module };
                if (prop.PropertyType.IsEnum)
                {
                    var items = prop.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static).Where(i => i.GetCustomAttribute<DisplayAttribute>() is not null);
                    item = new ComboBox
                    {
                        SelectedValuePath = "Key",
                        DisplayMemberPath = "Value",
                        ItemsSource = items.Select(i => new KeyValuePair<Enum, string>((Enum)i.GetValue(null)!, i.GetCustomAttribute<DisplayAttribute>()!.Name!)).ToArray()
                    };
                    item.SetBinding(ComboBox.SelectedValueProperty, bind);
                }
                else {
                    switch (prop.PropertyType.FullName)
                    {
                        case "System.DateOnly":
                            item = new DatePicker();
                            bind.Converter = new DateValueConverter();
                            item.SetBinding(DatePicker.SelectedDateProperty, bind);
                            break;
                        case "System.DateTime":
                            item = new DateTimePicker
                            {
                                Format = DateTimeFormat.SortableDateTime
                            };
                            item.SetBinding(DatePicker.SelectedDateProperty, bind);
                            break;
                        case "System.String":
                            item = new TextBox();
                            item.SetBinding(TextBox.TextProperty, bind);
                            break;
                        case "System.Boolean":
                            item = new ComboBox
                            {
                                SelectedValuePath = "Key",
                                DisplayMemberPath = "Value",
                                ItemsSource = new KeyValuePair<bool, string>[2] { new(false, "Нет"), new(true, "Да") }
                            };
                            item.SetBinding(ComboBox.SelectedValueProperty, bind);
                            break;
                        case "System.Int32":
                            item = new IntegerUpDown();
                            item.SetBinding(IntegerUpDown.ValueProperty, bind);
                            break;
                    }
                }
                control.AddItem(item);
                Items.Add(control.WrapBorder());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected override async void OnClosed(EventArgs e)
        {
            await CancelSource.CancelAsync();
            await ModuleCollection.Unload();
            base.OnClosed(e);
        }

        private void ParseStarted(int maxProgress)
        {
            MaxProgress = maxProgress;
            CurrProgress = 0;
        }

        private void ProgressUpdate(int value) => CurrProgress = value;

        private async void Cancel(object? s, RoutedEventArgs e)
        {
            await CancelSource.CancelAsync();
            OnPropertyChanged(nameof(IsEnabledCancelButton));
            OnPropertyChanged(nameof(IsEnabledStartButton));
            Module!.ParseStarted -= ParseStarted;
            Module!.ProgressUpdated -= ProgressUpdate;
        }

        private async void Start(object? s, RoutedEventArgs e)
        {
            CancelSource = new();
            Module!.ParseStarted += ParseStarted;
            Module!.ProgressUpdated += ProgressUpdate;
            OnPropertyChanged(nameof(IsEnabledCancelButton));
            OnPropertyChanged(nameof(IsEnabledStartButton));
            try
            {
                var receipts = await Module!.Parse(CancelSource.Token);
                CancelSource.Token.ThrowIfCancellationRequested();
                HistoryController.Add(new AddReceipts([.. receipts]));
                Cancel(null, new RoutedEventArgs());
                System.Windows.MessageBox.Show($"Парсинг завершен! Найдено чеков: {receipts.Count()}.");
            }
            catch (Exception ex)
            {
                if (!(ex is TaskCanceledException || ex is OperationCanceledException))
                    ErrorAlert(ex, "Не удалось спарсить чеки!");
                else Log.Information("Парсинг отменен пользователем.");
            }
            finally
            {
                Close();
            }
        }
    }
}
