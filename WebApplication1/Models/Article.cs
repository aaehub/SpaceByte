using System.Xml.Linq;

namespace WebApplication1.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public int AuthorID { get; set; }
        public DateTime PublicationDate { get; set; }

        //public virtual Users UserID { get; set; }
        //public virtual ICollection<Images> Images { get; set; }
        //public virtual ICollection<Videos> Videos { get; set; }
        //public virtual ICollection<APIMedia> APIMedias { get; set; }
        //public virtual ICollection<Paragraphs> Paragraphs { get; set; }
        //public virtual ICollection<Comments> Comments { get; set; }
    }
}
