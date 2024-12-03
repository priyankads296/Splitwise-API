using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Splitwise.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


        //    [JsonIgnore]                                        //Exclude GroupIdProperty from serialization that mean it will not be there if we return User objects                             
        //    public int? GroupId {  get; set; }                  //foreign key to group table

        //    [JsonIgnore]                                         //Exclude GroupProperty from serialization that mean it will not be there if we return User objects
        //    public Group Group {  get; set; }                   //Navigation property to group table
        //}
    }
}
