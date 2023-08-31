namespace WebApplication1.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public DateTime Date { get; set; }
        public string Comment_Text { get; set; }
        public int ArticleID { get; set; }
        public int UserID { get; set; }
    }
}
