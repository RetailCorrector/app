using RetailCorrector.Editor.Receipt.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailCorrector.Storage.Models
{
    [Table("code")]
    public class PositionCode
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("value")]
        public string Value { get; set; } = "";

        [ForeignKey(nameof(Position))]
        [Column("position")]
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;


        public static PositionCode[] Parse(RetailCorrector.PositionCode @struct, Position parent)
        {
            List<PositionCode> codes = [];
            if (!string.IsNullOrWhiteSpace(@struct.Unknown))
                codes.Add(new PositionCode { Type = 0, Value = @struct.Unknown, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.EAN8))
                codes.Add(new PositionCode { Type = 1, Value = @struct.EAN8, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.EAN13))
                codes.Add(new PositionCode { Type = 2, Value = @struct.EAN13, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.ITF14))
                codes.Add(new PositionCode { Type = 3, Value = @struct.ITF14, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.GS1_0))
                codes.Add(new PositionCode { Type = 4, Value = @struct.GS1_0, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.GS1_M))
                codes.Add(new PositionCode { Type = 5, Value = @struct.GS1_M, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.KMK))
                codes.Add(new PositionCode { Type = 6, Value = @struct.KMK, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.MI))
                codes.Add(new PositionCode { Type = 7, Value = @struct.MI, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.EGAIS2))
                codes.Add(new PositionCode { Type = 8, Value = @struct.EGAIS2, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.EGAIS3))
                codes.Add(new PositionCode { Type = 9, Value = @struct.EGAIS3, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F1))
                codes.Add(new PositionCode { Type = 10, Value = @struct.F1, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F2))
                codes.Add(new PositionCode { Type = 11, Value = @struct.F2, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F3))
                codes.Add(new PositionCode { Type = 12, Value = @struct.F3, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F4))
                codes.Add(new PositionCode { Type = 13, Value = @struct.F4, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F5))
                codes.Add(new PositionCode { Type = 14, Value = @struct.F5, Position = parent });
            if (!string.IsNullOrWhiteSpace(@struct.F6))
                codes.Add(new PositionCode { Type = 15, Value = @struct.F6, Position = parent });
            return [.. codes];
        }

        public static RetailCorrector.PositionCode ToStruct(IEnumerable<PositionCode> sql)
        {
            var vm = new CodeViewModel();
            foreach (var code in sql)
            {
                switch (code.Type)
                {
                    case 0:
                        vm.Unknown = code.Value;
                        break;
                    case 1:
                        vm.EAN8 = code.Value;
                        break;
                    case 2:
                        vm.EAN13 = code.Value;
                        break;
                    case 3:
                        vm.ITF14 = code.Value;
                        break;
                    case 4:
                        vm.GS1_0 = code.Value;
                        break;
                    case 5:
                        vm.GS1_M = code.Value;
                        break;
                    case 6:
                        vm.KMK = code.Value;
                        break;
                    case 7:
                        vm.MI = code.Value;
                        break;
                    case 8:
                        vm.EGAIS2 = code.Value;
                        break;
                    case 9:
                        vm.EGAIS3 = code.Value;
                        break;
                    case 10:
                        vm.F1 = code.Value;
                        break;
                    case 11:
                        vm.F2 = code.Value;
                        break;
                    case 12:
                        vm.F3 = code.Value;
                        break;
                    case 13:
                        vm.F4 = code.Value;
                        break;
                    case 14:
                        vm.F5 = code.Value;
                        break;
                    case 15:
                        vm.F6 = code.Value;
                        break;
                }
            }
            return vm;
        }
    }
}
