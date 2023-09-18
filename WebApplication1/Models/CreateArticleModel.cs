namespace WebApplication1.Models
{
    public class CreateArticleModel
    {

        public Article Article { get; set; }

        public Content content { get; set; }
        public List<Content> ContentList { get; set; }

        public CreateArticleModel()
        {
            ContentList = new List<Content>();
        }
    }
}
