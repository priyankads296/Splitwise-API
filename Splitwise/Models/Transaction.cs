namespace Splitwise.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
