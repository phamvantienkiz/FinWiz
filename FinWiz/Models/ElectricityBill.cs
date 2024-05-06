using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinWiz.Models
{
    public class ElectricityBill
    {
        [Key]
        public int ElectricityId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Address { get; set; }
        public int FirstReading { get; set; }
        public int LastReading { get; set; }
        public int TotalEnergy { get; set; }

        [Range(1, 1000000000, ErrorMessage = "Amount should be greater than 0.")] //10ty VNĐ
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        [Range(1, 1000000000, ErrorMessage = "TotalAmount should be greater than 0.")] //10ty VNĐ
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
