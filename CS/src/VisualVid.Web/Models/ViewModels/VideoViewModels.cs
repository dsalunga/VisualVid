using System.ComponentModel.DataAnnotations;

namespace VisualVid.Web.Models.ViewModels;

public class VideoUploadViewModel
{
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [StringLength(4000)]
    public string? Tags { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Video File")]
    public IFormFile? VideoFile { get; set; }
}

public class VideoEditViewModel
{
    public Guid VideoId { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [StringLength(4000)]
    public string? Tags { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public string? ThumbnailUrl { get; set; }
    public string? WatchUrl { get; set; }
}

public class WatchViewModel
{
    public WatchVideoDetail? Video { get; set; }
    public List<CommentViewModel> Comments { get; set; } = [];
    public List<VideoListItemViewModel> RelatedVideos { get; set; } = [];
}

public class WatchVideoDetail
{
    public Guid VideoId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? DateAdded { get; set; }
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
    public int Views { get; set; }
    public string? TagsHtml { get; set; }
    public string? CategoryName { get; set; }
}

public class CommentViewModel
{
    public int CommentId { get; set; }
    public string? Body { get; set; }
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
    public string? DateAdded { get; set; }
}

public class BrowseViewModel
{
    public List<VideoListItemViewModel> Videos { get; set; } = [];
    public List<Category> Categories { get; set; } = [];
    public int? SelectedCategoryId { get; set; }
    public string? Sort { get; set; }
}

public class VideoListItemViewModel
{
    public Guid VideoId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? DateAdded { get; set; }
    public int Views { get; set; }
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; }
}

public class SearchResultsViewModel
{
    public string? Query { get; set; }
    public string? Sort { get; set; }
    public List<VideoListItemViewModel> Videos { get; set; } = [];
}
