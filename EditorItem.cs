using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RetailCorrector.Wizard
{
    public class EditorItem : INotifyPropertyChanged
    {
        #region Изменяемые свойства
        public int DataTypeId
        {
            get => dataType;
            set
            {
                dataType = value;
                ConditionId = 0;
                PropertyId = 0;
                ConditionPattern = "";
                PropertyValue = "";
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentProperties));
            }
        }

        public int ConditionId
        {
            get => condition;
            set
            {
                condition = value;
                OnPropertyChanged();
            }
        }

        public string ConditionPattern
        {
            get => pattern;
            set
            {
                pattern = value;
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

        public string PropertyValue
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
        public string[] DataTypes { get; } = ["Чек", "Позиция"];
        private static readonly string[] PositionProperties = [
            "Название", "Цена", "Количество", "Сумма", 
            "Мера", "Способ оплаты", "Тип", "Налог"
            ];
        private static readonly string[] ReceiptProperties = [
            "Операция", "Итог", "Наличная", "Безналчиная", 
            "Предоплата", "Постоплата", "ВП"
            ];
        public string[] CurrentProperties => DataTypeId == 0 ? ReceiptProperties : PositionProperties;

        public string DateTypeName => DataTypes[DataTypeId];
        public string ConditionName => CurrentProperties[ConditionId];
        public string PropertyName => CurrentProperties[PropertyId];
        #endregion

        #region Приватные переменные
        private int dataType = 0;
        private int condition = 0;
        private string pattern = "";
        private int property = 0;
        private string value = "";
        #endregion

        #region Методы редактирования
        public void Edit(ref Receipt receipt)
        {
            switch (property)
            {
                case 0: receipt.Operation = (Operation)int.Parse(value); break;
                case 1: receipt.RoundedSum = uint.Parse(value); break;
                case 2:
                    var pay = receipt.Payment;
                    pay.Cash = uint.Parse(value);
                    receipt.Payment = pay;
                    break;
                case 3:
                    pay = receipt.Payment;
                    pay.ECash = uint.Parse(value);
                    receipt.Payment = pay;
                    break;
                case 4:
                    pay = receipt.Payment;
                    pay.Pre = uint.Parse(value);
                    receipt.Payment = pay;
                    break;
                case 5:
                    pay = receipt.Payment;
                    pay.Post = uint.Parse(value);
                    receipt.Payment = pay;
                    break;
                case 6:
                    pay = receipt.Payment;
                    pay.Provision = uint.Parse(value);
                    receipt.Payment = pay;
                    break;
                default: break;
            }
        }
        public void Edit(ref Position position)
        {
            switch (property)
            {
                case 0: position.Name = value; break;
                case 1: position.Price = uint.Parse(value); break;
                case 2:
                    position.Quantity = uint.Parse(value);
                    position.TotalSum = (uint)(position.Quantity * position.Price / 1000.0);
                    break;
                case 3: position.TotalSum = uint.Parse(value); break;
                case 4: position.MeasureUnit = (MeasureUnit)uint.Parse(value); break;
                case 5: position.PayType = (PaymentType)uint.Parse(value); break;
                case 6: position.PosType = (PositionType)uint.Parse(value); break;
                case 7: position.TaxRate = (TaxRate)uint.Parse(value); break;
                default: break;
            }
        }
        #endregion

        #region Методы проверки
        public bool Check(Position position) =>
            condition switch
            {
                0 => Regex.IsMatch(position.Name, pattern),
                1 => $"{position.Price}" == pattern,
                2 => $"{position.Quantity}" == pattern,
                3 => $"{position.TotalSum}" == pattern,
                4 => $"{position.MeasureUnit:D}" == pattern,
                5 => $"{position.PayType:D}" == pattern,
                6 => $"{position.PosType:D}" == pattern,
                7 => $"{position.TaxRate:D}" == pattern,
                _ => false
            };
        public bool Check(Receipt receipt) =>
            condition switch
            {
                0 => $"{receipt.Operation:D}" == pattern,
                1 => $"{receipt.RoundedSum}" == pattern,
                2 => $"{receipt.Payment.Cash}" == pattern,
                3 => $"{receipt.Payment.ECash}" == pattern,
                4 => $"{receipt.Payment.Pre}" == pattern,
                5 => $"{receipt.Payment.Post}" == pattern,
                6 => $"{receipt.Payment.Provision}" == pattern,
                _ => false
            };
        #endregion
    }
}
