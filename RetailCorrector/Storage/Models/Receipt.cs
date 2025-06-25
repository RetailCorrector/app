using System.ComponentModel.DataAnnotations.Schema;

namespace RetailCorrector.Storage.Models
{
    [Table("receipt")]
    public class Receipt
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("date")]
        public DateTime Created { get; set; }
        [Column("type")]
        public Operation Operation { get; set; }
        [Column("sign")]
        public string Sign { get; set; } = "";
        [Column("corr")]
        public CorrType CorrectionType { get; set; }
        [Column("act")]
        public string? ActNumber { get; set; }
        [Column("cash")]
        public uint CashSum { get; set; }
        [Column("ecash")]
        public uint ECashSum { get; set; }
        [Column("pre")]
        public uint PreSum { get; set; }
        [Column("post")]
        public uint PostSum { get; set; }
        [Column("other")]
        public uint ProvisionSum { get; set; }
        [Column("total")]
        public uint TotalSum { get; set; }

        public List<Position> Items { get; set; } = [];
        public List<IndustryData> IndustryData { get; set; } = [];

        public static implicit operator RetailCorrector.Receipt(Receipt sql) => new()
        {
            ActNumber = sql.ActNumber,
            TotalSum = sql.TotalSum,
            Operation = sql.Operation,
            Items = [.. sql.Items],
            CorrectionType = sql.CorrectionType,
            Created = sql.Created,
            FiscalSign = sql.Sign,
            IndustryData = [.. sql.IndustryData],
            Payment = new Payment
            {
                Cash = sql.CashSum,
                ECash = sql.ECashSum,
                Pre = sql.PreSum,
                Post = sql.PostSum,
                Provision = sql.ProvisionSum,
            }
        };

        public static implicit operator Receipt(RetailCorrector.Receipt @struct)
        {
            var res = new Receipt
            {
                ActNumber = @struct.ActNumber,
                TotalSum = @struct.TotalSum,
                CorrectionType = @struct.CorrectionType,
                Created = @struct.Created,
                CashSum = @struct.Payment.Cash,
                ECashSum = @struct.Payment.ECash,
                Operation = @struct.Operation,
                PostSum = @struct.Payment.Post,
                PreSum = @struct.Payment.Pre,
                ProvisionSum = @struct.Payment.Provision,
                Sign = @struct.FiscalSign
            };

            res.IndustryData = [.. @struct.IndustryData?.Select(d => new IndustryData(d, res)) ?? []];
            res.Items = [.. @struct.Items?.Select(d => new Position(d, res)) ?? []];

            return res;
        }            
    }
}
