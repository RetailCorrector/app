using Microsoft.Win32;
using System.Text.Json;

namespace RetailCorrector.Cashier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bufferMin.DataBindings.Add(new("Maximum", bufferMax, "Value"));
            bufferMax.DataBindings.Add(new("Minimum", bufferMin, "Value"));
            comSelector.Items.Clear();
            comSelector.Items.AddRange(coms);
        }

        private string[] coms
        {
            get
            {
                using var reg = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM");
                if (reg is null) return [];
                var names = reg.GetValueNames();
                var values = new string[names.Length];
                for (int i = 0; i < names.Length; i++)
                    values[i] = (string)reg.GetValue(names[i])!;
                return values;
            }
        }

        private void ShowOpenDialog(object sender, EventArgs e)
        {
            var res = folderDialog.ShowDialog();
            if (res == DialogResult.OK)
                pathInput.Text = folderDialog.SelectedPath;
        }

        private void Notify(string message) => MessageBox.Show(message);

        private List<Receipt> PullReceipts()
        {
            var res = new List<Receipt>();
            var files = Directory.GetFiles(Path.Combine(pathInput.Text, "tasks"));
            foreach (var file in files)
            {
                var text = File.ReadAllText(file);
                res.AddRange(JsonSerializer.Deserialize<Receipt[]>(text)!);
            }
            progress.Value = 0;
            progress.Maximum = res.Count;
            toolTip1.SetToolTip(progress, $"{progress.Value}/{progress.Maximum}");
            return res;
        }

        private async void Start(object sender, EventArgs e)
        {
            var receipts = PullReceipts();
            var _module = new ModuleLoadContext();
            using (var fs = File.Open(pathDriver.Text, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var assembly = _module.LoadFromStream(fs);
                var moduleType = assembly.GetTypes().FirstOrDefault(t => t.BaseType == typeof(AbstractFiscalModule));
                var module = (AbstractFiscalModule)Activator.CreateInstance(moduleType!)!;
                module.OnNotify += Notify;
                moduleType!.GetProperty("ComFile")!.SetValue(module, comSelector.Text);
                if (await module.OnLoad(_module))
                {
                    foreach (var receipt in receipts)
                    {
                        if(await module.CountUnsendDocs() > bufferMax.Value)
                        {
                            do
                            {
                                await Task.Delay(1000);
                            } while (await module.CountUnsendDocs() <= bufferMin.Value);
                        }
                        var res = await module.ProcessingReceipt(receipt);
                        if (!res)
                        {
                            MessageBox.Show("Не удалось отбить чек! Подробнее в лог-файле...");
                            break;
                        }
                        else
                        {
                            progress.Value++;
                            toolTip1.SetToolTip(progress, $"{progress.Value}/{progress.Maximum}");
                        }
                    }
                }
                await module.OnUnload();
                module.OnNotify -= Notify;
                module = null;
                moduleType = null;
                assembly = null;
            }
            _module.Unload();
            _module = null;
        }

        private void ShowOpenDllDialog(object sender, EventArgs e)
        {
            var res = openFiscalDriver.ShowDialog();
            if (res == DialogResult.OK)
                pathDriver.Text = openFiscalDriver.FileName;
        }
    }
}
