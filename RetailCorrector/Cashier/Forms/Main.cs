using RetailCorrector.Cashier.Extensions;
using RetailCorrector.Cashier.ModuleSystem;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace RetailCorrector.Cashier.Forms
{
    public partial class Main : Form, INotifyPropertyChanged
    {
        private CancellationTokenSource _cancelSource = new();

        private void UpdateButtons()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartEnabled)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CancelEnabled)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool StartEnabled => _cancelSource.IsCancellationRequested;
        public bool CancelEnabled => !_cancelSource.IsCancellationRequested;

        public Main()
        {
            InitializeComponent();
            btnCancel.Enabled = false;
            btnCancel.DataBindings.Add(new Binding("Enabled", this, "CancelEnabled"));
            btnStart.DataBindings.Add(new Binding("Enabled", this, "StartEnabled"));
            table.DataBindings.Add(new Binding("Enabled", this, "StartEnabled"));
            Cancel();
        }
        protected override async void OnShown(EventArgs e)
        {
            await ModuleCollection.Load();
            base.OnShown(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        private void Cancel()
        {
            _cancelSource.Cancel();
            UpdateButtons();
        }

        private void ModuleSelected(object sender, EventArgs e)
        {
            table.Controls.Clear();
            var module = ModuleCollection.Modules[fiscalModules.SelectedIndex];
            table.DataContext = module.EntryPoint;
            var type = module.EntryPoint!.GetType();
            var props = type.GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>() is not null).ToArray();
            for(var i = 0; i < props.Length; i++)
            {
                var name = props[i].GetCustomAttribute<DisplayNameAttribute>()!.DisplayName;
                var value = AutoSelectControl(props[i]);
                value.Width = 168;
                var lbl = new Label
                {
                    Text = name,
                    AutoSize = false,
                    Height = value.Height,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Width = 168,
                };
                table.Controls.Add(lbl, 0, i);
                table.Controls.Add(value, 1, i);
            }
        }

        private Control AutoSelectControl(PropertyInfo info)
        {
            var type = info.PropertyType;
            if (type.IsEnum)
            {
                var combobox = new ComboBox();
                var names = EnumHelper.GetDisplayNames(type);
                combobox.DisplayMember = "Value";
                combobox.Name = info.Name;
                combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                combobox.SelectedValueChanged += (s, e) =>
                {
                    var _combobox = (ComboBox)s!;
                    var type = table.DataContext!.GetType();
                    var prop = type.GetProperty(_combobox.Name)!;
                    var value = ((KeyValuePair<object, string>)_combobox.SelectedItem!).Key;
                    prop.SetValue(table.DataContext, value);
                };
                foreach (var name in names)
                    combobox.Items.Add(name);
                return combobox;
            }
            if (type == typeof(int))
            {
                var control = new NumericUpDown();
                control.Maximum = 1_000_000_000;
                control.DataBindings.Add(new Binding("Value", table.DataContext, info.Name));
                return control;
            }
            var box = new TextBox();
            box.DataBindings.Add(new Binding("Text", table.DataContext, info.Name));
            return box;
        }

        private async void RefreshModules(object sender, EventArgs e)
        {
            await ModuleCollection.Unload();
            await ModuleCollection.Load();
        }

        private List<Receipt> PullReceipts()
        {
            var res = new List<Receipt>();
            var files = Directory.GetFiles(Pathes.Receipts);
            foreach (var file in files)
        {
                var text = File.ReadAllText(file);
                res.AddRange(JsonSerializer.Deserialize<Receipt[]>(text)!);
            }
            progress.Value = 0;
            progress.Maximum = res.Count;
            return res;
        }

        private async void Start(object sender, EventArgs e)
        {
            _cancelSource = new();
            UpdateButtons();
            ((Control)sender).Enabled = false;
            var receipts = PullReceipts();
            var module = ModuleCollection.Modules[fiscalModules.SelectedIndex].EntryPoint!;
            try
            {
                if (!await module.Connect())
                    throw new Exception("Не удалось подключиться...");
                foreach (var receipt in receipts)
                {
                    if (await module.CountUnsendDocs() > maxSizeBuffer.Value)
                    {
                        do { await Task.Delay(1000); } 
                        while (await module.CountUnsendDocs() <= minSizeBuffer.Value);
                    }
                    var res = await module.ProcessingReceipt(receipt);
                    if (!res)
                        throw new Exception("Не удалось отбить чек! Подробнее в лог-файле...");
                    progress.Value++;
                }
            }
            catch (Exception ex)
            {
                await module.Disconnect();
                MessageBox.Show(ex.Message);
                Cancel();
            }
        }

        private void OpenDocs(object sender, CancelEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true });
    }
}
