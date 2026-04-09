namespace VisualVid.Web.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }

    public ICollection<Video> Videos { get; set; } = [];
}
