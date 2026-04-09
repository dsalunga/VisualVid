namespace VisualVid.Web.Models;

public class Member
{
    public Guid UserId { get; set; }
    public bool Gender { get; set; }
    public int CountryCode { get; set; }
    public DateTime BirthDate { get; set; }
    public int Watched { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Navigation
    public Country? Country { get; set; }
    public ApplicationUser? User { get; set; }
}
