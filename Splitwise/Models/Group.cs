using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Models
{
    public class Group
    {
       
        public int GroupId { get; set; }
       // public int UserId { get; set; }
       //public string Title { get; set; }
       //public int GroupDetailId { get; set; }
        [ForeignKey("GroupDetailId")]
        public GroupDetail GroupDetails { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
