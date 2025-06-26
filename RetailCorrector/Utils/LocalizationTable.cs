using System.Data;

namespace RetailCorrector.Utils
{
    public static class LocalizationTable
    {
        public static DataTable Locale(this DataTable table) => table.TableName switch
        {
            "receipt" => LocalReceipt(table),
            "position" => LocalPosition(table),
            "industry" => LocalIndustry(table),
            "code" => LocalPosCode(table),
            _ => table
        };

        private static DataTable LocalReceipt(DataTable table)
        {
            table.Columns[0].ColumnName = "№";
            table.Columns[1].ColumnName = "Дата";
            table.Columns[2].ColumnName = "Операция чека";
            table.Columns[3].ColumnName = "Фискальный признак";
            table.Columns[4].ColumnName = "Тип коррекции";
            table.Columns[5].ColumnName = "Номер документа";
            table.Columns[6].ColumnName = "Наличная оплата";
            table.Columns[7].ColumnName = "Безналичная оплата";
            table.Columns[8].ColumnName = "Предоплата";
            table.Columns[9].ColumnName = "Постоплата";
            table.Columns[10].ColumnName = "Иная оплата";
            table.Columns[11].ColumnName = "Итоговая сумма";
            return table;
        }

        private static DataTable LocalIndustry(DataTable table)
        {
            table.Columns[0].ColumnName = "№";
            table.Columns[1].ColumnName = "Идентификатор ФОИВ";
            table.Columns[2].ColumnName = "Дата документа основания";
            table.Columns[3].ColumnName = "Номер документа основания";
            table.Columns[4].ColumnName = "Значение";
            table.Columns[5].ColumnName = "№ позиции";
            table.Columns[6].ColumnName = "№ чека";
            return table;
        }

        private static DataTable LocalPosition(DataTable table)
        {
            table.Columns[0].ColumnName = "№";
            table.Columns[1].ColumnName = "Наименование";
            table.Columns[2].ColumnName = "Метод оплаты";
            table.Columns[3].ColumnName = "Тип позиции";
            table.Columns[4].ColumnName = "Ставка НДС";
            table.Columns[5].ColumnName = "Единица измерения";
            table.Columns[6].ColumnName = "Цена";
            table.Columns[7].ColumnName = "Количество";
            table.Columns[8].ColumnName = "Стоимость";
            table.Columns[9].ColumnName = "№ чека";
            return table;
        }

        private static DataTable LocalPosCode(DataTable table)
        {
            table.Columns[0].ColumnName = "№";
            table.Columns[1].ColumnName = "Вид кода";
            table.Columns[2].ColumnName = "Код";
            table.Columns[3].ColumnName = "№ позиции";
            return table;
        }
    }
}
