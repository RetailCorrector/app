using System.ComponentModel.DataAnnotations.Schema;

namespace RetailCorrector.Storage.Models
{
    [Table("industry")]
    public class IndustryData
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("gos")]
        public int GosId { get; set; }
        [Column("date")]
        public DateOnly DocDate { get; set; }
        [Column("number")]
        public int DocNumb { get; set; }
        [Column("value")]
        public string DocValue { get; set; } = "";

        [ForeignKey(nameof(Position))]
        [Column("position")]
        public int? PositionId { get; set; }
        public Position? Position { set; get; }

        [ForeignKey(nameof(Receipt))]
        [Column("receipt")]
        public int? ReceiptId { get; set; }
        public Receipt? Receipt { set; get; }

        public IndustryData() { }
        private IndustryData(RetailCorrector.IndustryData @struct)
        {
            GosId = @struct.GosId;
            DocDate = @struct.Date;
            DocNumb = @struct.Number;
            DocValue = @struct.Value;
        }
        public IndustryData(RetailCorrector.IndustryData @struct, Receipt parent) : 
            this(@struct) => Receipt = parent;
        public IndustryData(RetailCorrector.IndustryData @struct, Position parent) :
            this(@struct) => Position = parent;


        public static implicit operator RetailCorrector.IndustryData(IndustryData sql) => new()
        {
            GosId = (byte)sql.GosId,
            Date = sql.DocDate,
            Number = sql.DocNumb,
            Value = sql.DocValue,
        };
    }
}
