using RetailCorrector.Cashier.Extensions;
using RetailCorrector.Cashier.ModuleSystem;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace RetailCorrector.Cashier.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        protected override async void OnShown(EventArgs e)
        {
            await ModuleCollection.Load();
            base.OnShown(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

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

        private static Control AutoSelectControl(PropertyInfo info)
        {
            var type = info.PropertyType;
            if (type.IsEnum)
            {
                var combobox = new ComboBox();
                var names = ((KeyValuePair<Enum, string>[])EnumHelper.GetDisplayNames(type)).Select(p => p.Value);
                foreach(var name in names)
                    combobox.Items.Add(name);
                return combobox;
            }
            if (type == typeof(int))
                return new NumericUpDown();
            return new TextBox();
        }

        private async void RefreshModules(object sender, EventArgs e)
        {
            await ModuleCollection.Unload();
            await ModuleCollection.Load();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void OpenDocs(object sender, CancelEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true });
    }
}
