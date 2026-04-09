using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Data;
using VisualVid.Web.Models;

namespace VisualVid.Web.Services;

public class MemberService
{
    private readonly ApplicationDbContext _db;

    public MemberService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Member?> GetByUserIdAsync(Guid userId)
    {
        return await _db.Members
            .Include(m => m.Country)
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task CreateOrUpdateAsync(Guid userId, int countryCode, bool gender, DateTime birthDate)
    {
        var member = await _db.Members.FindAsync(userId);
        if (member == null)
        {
            member = new Member
            {
                UserId = userId,
                CountryCode = countryCode,
                Gender = gender,
                BirthDate = birthDate,
                Watched = 0
            };
            _db.Members.Add(member);
        }
        else
        {
            member.CountryCode = countryCode;
            member.Gender = gender;
            member.BirthDate = birthDate;
        }

        await _db.SaveChangesAsync();
    }

    public async Task IncrementWatchedAsync(Guid userId)
    {
        await _db.Members
            .Where(m => m.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.Watched, m => m.Watched + 1));
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _db.Categories.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        return await _db.Countries.OrderBy(c => c.CountryCode).ToListAsync();
    }

    public static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}
