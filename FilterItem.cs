using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RetailCorrector.Wizard
{
    public class FilterItem : INotifyPropertyChanged
    {
        #region Изменяемые свойства
        public bool IsEnabled
        {
            get => enabled;
            set
            {
                enabled = value;
                OnPropertyChanged();
            }
        }

        public int PropertyId
        {
            get => property;
            set
            {
                property = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Обновление свойств
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Локализация
        public string[] Properties { get; } = [
            "Дата", "Операция", "Итог", 
            "Наличные", "Безналичные", "Предоплата", "Постоплата", "ВП",
            "Наименование", "Цена позиции", "Кол-во в позиции", "Сумма позиции",
            "Мера позиции", "Налог позиции", "Тип позиции", "Оплата позции"
            ];

        public string PropertyName => Properties[PropertyId];
        #endregion

        #region Приватные переменные
        private bool enabled = false;
        private int property = 0;
        private string value = "";
        #endregion

        public bool Check(Receipt receipt)
        {
            try
            {
                return !enabled || property switch
                {
                    0 => receipt.Created.ToString("yyyy'-'MM'-'dd") == value,
                    1 => $"{receipt.Operation:D}" == value,
                    2 => receipt.RoundedSum == (uint)Math.Round(double.Parse(value) * 100),
                    3 => receipt.Payment.Cash == (uint)Math.Round(double.Parse(value) * 100),
                    4 => receipt.Payment.ECash == (uint)Math.Round(double.Parse(value) * 100),
                    5 => receipt.Payment.Pre == (uint)Math.Round(double.Parse(value) * 100),
                    6 => receipt.Payment.Post == (uint)Math.Round(double.Parse(value) * 100),
                    7 => receipt.Payment.Provision == (uint)Math.Round(double.Parse(value) * 100),
                    8 => receipt.Items.Any(i => Regex.IsMatch(i.Name, value)),
                    9 => receipt.Items.Any(i => i.Price == (uint)Math.Round(double.Parse(value) * 100)),
                    10 => receipt.Items.Any(i => i.Quantity == (uint)Math.Round(double.Parse(value) * 1000)),
                    11 => receipt.Items.Any(i => i.TotalSum == (uint)Math.Round(double.Parse(value) * 100)),
                    12 => receipt.Items.Any(i => $"{i.MeasureUnit:D}" == value),
                    13 => receipt.Items.Any(i => $"{i.TaxRate:D}" == value),
                    14 => receipt.Items.Any(i => $"{i.PosType:D}" == value),
                    15 => receipt.Items.Any(i => $"{i.PayType:D}" == value),
                    _ => false,
                };
            }
            catch { return false; }
        }
    }
}
