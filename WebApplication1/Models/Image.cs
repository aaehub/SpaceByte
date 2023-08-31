namespace WebApplication1.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public int ArticleID { get; set; }
        public string ImageURL { get; set; }
        public string Caption { get; set; }
        public int OrderNumber { get; set; }

    }
}
