using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string GroupName { get; set; }
        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        //bool isSettled = false;
        //[NotMapped]
        public ICollection<User> UsersPaid { get; set; }
        
        public ICollection<User> UsersInvolved { get; set; }
        //public Dictionary<User,Balance> Balances { get; set; }

        public ExpenseDetail ExpenseDetails { get; set; }
        //public int GroupId { get; set; }

        ////[ForeignKey("GroupId")]
        ////public Group Group { get; set; }

        //public int UserId { get; set; }
        //public string Title { get; set; }
        //[ForeignKey("UserId")]
        //public User User { get; set; }

    }
}
