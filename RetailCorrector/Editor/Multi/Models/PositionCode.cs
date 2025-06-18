using RetailCorrector.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailCorrector.Editor.Multi.Models
{
    [Table("code")]
    public class PositionCode
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("type")]
        public PositionCodeType Type { get; set; }
        [Column("value")]
        public string Value { get; set; } = "";

        [ForeignKey(nameof(Position))]
        [Column("position")]
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;

        public PositionCode() { }
        public PositionCode(RetailCorrector.PositionCode @struct, Position parent)
        {
            Position = parent;
            Type = @struct.Type;
            Value = @struct.Value;
        }

        public static implicit operator RetailCorrector.PositionCode(PositionCode sql) => 
            new(sql.Value, sql.Type);
    }
}
