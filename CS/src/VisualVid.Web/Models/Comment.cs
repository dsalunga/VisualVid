namespace VisualVid.Web.Models;

public class Comment
{
    public int CommentId { get; set; }
    public DateTime? DatePosted { get; set; }
    public string? Content { get; set; }
    public int? ReplyFromId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? VideoId { get; set; }

    // Navigation
    public ApplicationUser? User { get; set; }
    public Video? Video { get; set; }
}
