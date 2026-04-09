namespace VisualVid.Web.Models;

public class Video
{
    public Guid VideoId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DateAdded { get; set; }
    public string? Tags { get; set; }
    public int Views { get; set; }
    public int Ratings { get; set; }
    public int RatingTicks { get; set; }
    public int? CategoryId { get; set; }
    public int Length { get; set; }
    public bool IsActive { get; set; }
    public string? OriginalExtension { get; set; }
    public Guid? UserId { get; set; }
    public bool Pending { get; set; }

    // Navigation
    public Category? Category { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<Comment> Comments { get; set; } = [];
}
