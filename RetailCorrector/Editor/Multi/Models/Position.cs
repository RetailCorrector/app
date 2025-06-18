using System.ComponentModel.DataAnnotations.Schema;

namespace RetailCorrector.Editor.Multi.Models
{
    [Table("position")]
    public class Position
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("payment")]
        public PaymentType PayType { get; set; }
        [Column("type")]
        public PositionType PosType { get; set; }
        [Column("tax")]
        public TaxRate TaxRate { get; set; }
        [Column("unit")]
        public MeasureUnit? Measure { get; set; }
        [Column("price")]
        public uint Price { get; set; }
        [Column("quantity")]
        public uint Quantity { get; set; }
        [Column("total")]
        public uint TotalSum { get; set; }

        public List<IndustryData> IndustryData { get; set; } = [];
        public List<PositionCode> Codes { get; set; } = [];

        [ForeignKey(nameof(Receipt))]
        [Column("receipt")]
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; } = null!;

        public Position() { }
        public Position(RetailCorrector.Position @struct, Receipt parent)
        {
            Name = @struct.Name;
            PayType = @struct.PayType;
            PosType = @struct.PosType;
            TaxRate = @struct.TaxRate;
            Measure = @struct.MeasureUnit;
            Price = @struct.Price;
            Quantity = @struct.Quantity;
            TotalSum = @struct.TotalSum;
            Receipt = parent;
        }

        public static implicit operator RetailCorrector.Position(Position sql) => new()
        {
            Name = sql.Name ?? "",
            Price = sql.Price,
            Quantity = sql.Quantity,
            TotalSum = sql.TotalSum,
            PayType = sql.PayType,
            MeasureUnit = sql.Measure ?? MeasureUnit.None,
            PosType = sql.PosType,
            TaxRate = sql.TaxRate,
            IndustryData = [.. sql.IndustryData],
            Codes = [.. sql.Codes],
        };
    }
}
