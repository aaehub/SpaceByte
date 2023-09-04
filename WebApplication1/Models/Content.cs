using MessagePack;

namespace WebApplication1.Models
{
    public class Content
        {


        
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string content { get; set; }
        public string ContentType { get; set; }


        public int ArticleID { get; set; }
        public Article Article { get; set; }

    }
}
