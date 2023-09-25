using System.Xml.Linq;

namespace WebApplication1.Models
{
    public class User
    {

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public Boolean Status { get; set; }
        public DateTime DateCreated { get; set; }


        

        //public virtual ICollection<Article> Articles { get; set; }
        //public virtual ICollection<Comments> Comments { get; set; }
    }
}
