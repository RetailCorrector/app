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

        public bool Check(Receipt receipt) => !enabled || property switch
        {
            0 => receipt.Created.ToString("yyyy'-'MM'-'dd") == value,
            1 => $"{receipt.Operation:D}" == value,
            2 => $"{receipt.RoundedSum}" == value,
            3 => $"{receipt.Payment.Cash}" == value,
            4 => $"{receipt.Payment.ECash}" == value,
            5 => $"{receipt.Payment.Pre}" == value,
            6 => $"{receipt.Payment.Post}" == value,
            7 => $"{receipt.Payment.Provision}" == value,
            8 => receipt.Items.Any(i => Regex.IsMatch(i.Name, value)),
            9 => receipt.Items.Any(i => $"{i.Price}" == value),
            10 => receipt.Items.Any(i => $"{i.Quantity}" == value),
            11 => receipt.Items.Any(i => $"{i.TotalSum}" == value),
            12 => receipt.Items.Any(i => $"{i.MeasureUnit:D}" == value),
            13 => receipt.Items.Any(i => $"{i.TaxRate:D}" == value),
            14 => receipt.Items.Any(i => $"{i.PosType:D}" == value),
            15 => receipt.Items.Any(i => $"{i.PayType:D}" == value),
            _ => false,
        };
    }
}
