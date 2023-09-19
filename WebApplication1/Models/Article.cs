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
      

        // Other properties of the Article class

        public List<Content> ContentList { get; set; }
    }
}