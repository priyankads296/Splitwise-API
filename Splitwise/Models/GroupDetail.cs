namespace Splitwise.Models
{
    public class GroupDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category {  get; set; }
        public DateTime CreatedOn = DateTime.Now;
        public string CreatedBy { get; set; }
        


    }
}
