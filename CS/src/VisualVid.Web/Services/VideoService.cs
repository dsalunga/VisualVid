using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Data;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;

namespace VisualVid.Web.Services;

public class VideoService
{
    private readonly ApplicationDbContext _db;

    public VideoService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<VideoListItemViewModel>> GetFeaturedVideosAsync(int count = 10)
    {
        return await _db.Videos
            .Where(v => v.IsActive)
            .OrderByDescending(v => v.DateAdded)
            .Take(count)
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<List<VideoListItemViewModel>> GetMostWatchedAsync(int count = 10)
    {
        return await _db.Videos
            .Where(v => v.IsActive)
            .OrderByDescending(v => v.Views)
            .ThenByDescending(v => v.DateAdded)
            .Take(count)
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<List<VideoListItemViewModel>> GetByUserAsync(Guid userId, bool activeOnly = false)
    {
        var query = _db.Videos.Where(v => v.UserId == userId);
        if (activeOnly)
            query = query.Where(v => v.IsActive);

        return await query
            .OrderByDescending(v => v.DateAdded)
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<List<VideoListItemViewModel>> GetByCategoryAsync(int categoryId, bool activeOnly = true)
    {
        var query = _db.Videos.Where(v => v.CategoryId == categoryId);
        if (activeOnly)
            query = query.Where(v => v.IsActive);

        return await query
            .OrderByDescending(v => v.DateAdded)
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<List<VideoListItemViewModel>> SearchAsync(string keyword, string? sort = null)
    {
        var query = _db.Videos
            .Where(v => v.IsActive && v.Tags != null && v.Tags.Contains(keyword));

        query = sort switch
        {
            "Views" => query.OrderByDescending(v => v.Views),
            _ => query.OrderByDescending(v => v.DateAdded)
        };

        return await query
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<List<VideoListItemViewModel>> GetAllActiveAsync(string? sort = null)
    {
        var query = _db.Videos.Where(v => v.IsActive);

        query = sort switch
        {
            "Views" => query.OrderByDescending(v => v.Views),
            _ => query.OrderByDescending(v => v.DateAdded)
        };

        return await query
            .Include(v => v.User)
            .Select(v => ToListItem(v))
            .ToListAsync();
    }

    public async Task<Video?> GetByIdAsync(Guid videoId, bool incrementViews = false)
    {
        if (incrementViews)
        {
            await _db.Videos
                .Where(v => v.VideoId == videoId)
                .ExecuteUpdateAsync(s => s.SetProperty(v => v.Views, v => v.Views + 1));
        }

        return await _db.Videos
            .Include(v => v.User)
            .Include(v => v.Category)
            .FirstOrDefaultAsync(v => v.VideoId == videoId);
    }

    public async Task<Guid> CreateAsync(string title, string? description, string? tags,
        int categoryId, string originalExtension, Guid userId)
    {
        var video = new Video
        {
            VideoId = Guid.NewGuid(),
            Title = title,
            Description = description,
            Tags = tags,
            CategoryId = categoryId,
            OriginalExtension = originalExtension,
            UserId = userId,
            DateAdded = DateTime.UtcNow,
            IsActive = false,
            Pending = true,
            Views = 0,
            Ratings = 0,
            RatingTicks = 0,
            Length = 0
        };

        _db.Videos.Add(video);
        await _db.SaveChangesAsync();
        return video.VideoId;
    }

    public async Task UpdateAsync(Guid videoId, string title, string? description, string? tags, int categoryId)
    {
        await _db.Videos
            .Where(v => v.VideoId == videoId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(v => v.Title, title)
                .SetProperty(v => v.Description, description)
                .SetProperty(v => v.Tags, tags)
                .SetProperty(v => v.CategoryId, categoryId));
    }

    public async Task DeleteAsync(Guid videoId)
    {
        await _db.Videos
            .Where(v => v.VideoId == videoId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<CommentViewModel>> GetCommentsAsync(Guid videoId)
    {
        return await _db.Comments
            .Where(c => c.VideoId == videoId)
            .Include(c => c.User)
            .OrderByDescending(c => c.DatePosted)
            .Select(c => new CommentViewModel
            {
                CommentId = c.CommentId,
                Body = c.Content,
                UserName = c.User != null ? c.User.UserName : "Unknown",
                UserId = c.UserId ?? Guid.Empty,
                DateAdded = c.DatePosted != null ? c.DatePosted.Value.ToString("MMMM d, yyyy h:mm tt") : ""
            })
            .ToListAsync();
    }

    public async Task AddCommentAsync(Guid videoId, Guid userId, string content)
    {
        var comment = new Comment
        {
            VideoId = videoId,
            UserId = userId,
            Content = content,
            DatePosted = DateTime.UtcNow
        };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
    }

    private static VideoListItemViewModel ToListItem(Video v) => new()
    {
        VideoId = v.VideoId,
        Title = v.Title,
        Description = v.Description,
        DateAdded = v.DateAdded?.ToString("MMMM d, yyyy"),
        Views = v.Views,
        UserName = v.User?.UserName,
        UserId = v.UserId ?? Guid.Empty,
        ThumbnailUrl = $"/videos/members/{v.UserId}/{v.VideoId}.jpg",
        IsActive = v.IsActive
    };
}
