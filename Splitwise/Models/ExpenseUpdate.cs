using System.ComponentModel.DataAnnotations;

namespace Splitwise.Models
{
    public class ExpenseUpdate
    {
        [Key]
        public int Id { get; set; }
        public string? GroupName { get; set; }
        //[NotMapped]
        //public ICollection<User>? UsersPaid { get; set; }

        public ICollection<User> UsersInvolved { get; set; }

        public ExpenseDetail ExpenseDetails { get; set; }
        //public int GroupId { get; set; }

    }
}
