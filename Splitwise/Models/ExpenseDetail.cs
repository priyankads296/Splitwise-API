using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Splitwise.Models
{
    public class ExpenseDetail
    {
        public int Id { get; set; }
        [ForeignKey("ExpenseId")]
        public int ExpenseId { get; set; }
        [JsonIgnore]
        public Expense? Expense { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn =DateTime.Now;
    }
}
