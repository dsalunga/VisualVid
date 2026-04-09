using System.ComponentModel.DataAnnotations;

namespace VisualVid.Web.Models.ViewModels;

public class MemberDashboardViewModel
{
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; }
    public string? MemberSince { get; set; }
    public int Watched { get; set; }
    public int TotalVideos { get; set; }
    public int TotalViews { get; set; }
    public List<VideoListItemViewModel> Videos { get; set; } = [];
}

public class MemberProfileEditViewModel
{
    public string? UserName { get; set; }
    public string? PhotoUrl { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Country")]
    public int? CountryId { get; set; }

    [Display(Name = "About Me")]
    public string? AboutMe { get; set; }

    [Display(Name = "Profile Photo")]
    public IFormFile? Photo { get; set; }
}

public class PublicProfileViewModel
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Location { get; set; }
    public string? AboutMe { get; set; }
    public string? MemberSince { get; set; }
    public List<VideoListItemViewModel> Videos { get; set; } = [];
}
