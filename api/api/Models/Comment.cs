namespace api.Models;

public class Comment
{
    public string CommentContent { get; set; } = string.Empty;
    public int Likes { get; set; }
}