namespace WebApplication1.Models
{
    public class CreateArticleModel
    {

        public Article Article { get; set; }
        public List<Content> Contents { get; set; }

        public CreateArticleModel()
        {
            Contents = new List<Content>();
        }
    }
}
