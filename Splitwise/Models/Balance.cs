using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Models
{
    public class Balance
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        ////[ForeignKey("UserId")]
        ////public User User { get; set; }
        public decimal Amount { get; set; }
    }
}
