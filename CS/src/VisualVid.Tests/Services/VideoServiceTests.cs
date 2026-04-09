using Xunit;
using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Data;
using VisualVid.Web.Models;
using VisualVid.Web.Services;

namespace VisualVid.Tests.Services;

public class VideoServiceTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly VideoService _service;
    private readonly Guid _userId = Guid.NewGuid();

    public VideoServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"VisualVidTest_{Guid.NewGuid()}")
            .Options;

        _db = new ApplicationDbContext(options);
        _service = new VideoService(_db);

        SeedData();
    }

    private void SeedData()
    {
        var user = new ApplicationUser
        {
            Id = _userId,
            UserName = "testuser",
            NormalizedUserName = "TESTUSER",
            Email = "test@example.com",
            NormalizedEmail = "TEST@EXAMPLE.COM"
        };
        _db.Users.Add(user);

        var category = new Category { CategoryId = 1, Name = "Music" };
        _db.Categories.Add(category);

        _db.Videos.AddRange(
            new Video
            {
                VideoId = Guid.NewGuid(),
                Title = "Active Video 1",
                Description = "Description 1",
                Tags = "music rock",
                CategoryId = 1,
                UserId = _userId,
                IsActive = true,
                Pending = false,
                Views = 100,
                DateAdded = DateTime.UtcNow.AddDays(-1)
            },
            new Video
            {
                VideoId = Guid.NewGuid(),
                Title = "Active Video 2",
                Description = "Description 2",
                Tags = "dance pop",
                CategoryId = 1,
                UserId = _userId,
                IsActive = true,
                Pending = false,
                Views = 50,
                DateAdded = DateTime.UtcNow.AddDays(-2)
            },
            new Video
            {
                VideoId = Guid.NewGuid(),
                Title = "Pending Video",
                Description = "Not yet active",
                Tags = "pending",
                CategoryId = 1,
                UserId = _userId,
                IsActive = false,
                Pending = true,
                Views = 0,
                DateAdded = DateTime.UtcNow
            }
        );

        _db.SaveChanges();
    }

    [Fact]
    public async Task GetFeaturedVideosAsync_ReturnsOnlyActiveVideos()
    {
        var result = await _service.GetFeaturedVideosAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.True(v.IsActive));
    }

    [Fact]
    public async Task GetFeaturedVideosAsync_OrderedByDateDescending()
    {
        var result = await _service.GetFeaturedVideosAsync();

        Assert.Equal("Active Video 1", result[0].Title);
        Assert.Equal("Active Video 2", result[1].Title);
    }

    [Fact]
    public async Task GetMostWatchedAsync_OrderedByViewsDescending()
    {
        var result = await _service.GetMostWatchedAsync();

        Assert.Equal(100, result[0].Views);
        Assert.Equal(50, result[1].Views);
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsAllUserVideos()
    {
        var result = await _service.GetByUserAsync(_userId);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByUserAsync_ActiveOnly_FiltersCorrectly()
    {
        var result = await _service.GetByUserAsync(_userId, activeOnly: true);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByCategoryAsync_FiltersCorrectly()
    {
        var result = await _service.GetByCategoryAsync(1);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchAsync_FindsByTag()
    {
        var result = await _service.SearchAsync("rock");
        Assert.Single(result);
        Assert.Equal("Active Video 1", result[0].Title);
    }

    [Fact]
    public async Task SearchAsync_NoMatch_ReturnsEmpty()
    {
        var result = await _service.SearchAsync("nonexistent");
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_AddsVideoToDatabase()
    {
        var videoId = await _service.CreateAsync("New Video", "Desc", "tag1", 1, ".mp4", _userId);

        var video = await _db.Videos.FindAsync(videoId);
        Assert.NotNull(video);
        Assert.Equal("New Video", video.Title);
        Assert.False(video.IsActive);
        Assert.True(video.Pending);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsVideo()
    {
        var existing = await _db.Videos.FirstAsync(v => v.IsActive);
        var result = await _service.GetByIdAsync(existing.VideoId);

        Assert.NotNull(result);
        Assert.Equal(existing.Title, result.Title);
    }

    [Fact(Skip = "ExecuteUpdate not supported by InMemory provider - tested via integration")]
    public async Task GetByIdAsync_WithIncrementViews_IncrementsViewCount()
    {
        var existing = await _db.Videos.FirstAsync(v => v.IsActive);
        var originalViews = existing.Views;

        await _service.GetByIdAsync(existing.VideoId, incrementViews: true);

        await _db.Entry(existing).ReloadAsync();
        Assert.Equal(originalViews + 1, existing.Views);
    }

    [Fact(Skip = "ExecuteDelete not supported by InMemory provider - tested via integration")]
    public async Task DeleteAsync_RemovesVideo()
    {
        var existing = await _db.Videos.FirstAsync();
        await _service.DeleteAsync(existing.VideoId);

        var deleted = await _db.Videos.FindAsync(existing.VideoId);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task AddCommentAsync_CreatesComment()
    {
        var video = await _db.Videos.FirstAsync();
        await _service.AddCommentAsync(video.VideoId, _userId, "Great video!");

        var comments = await _service.GetCommentsAsync(video.VideoId);
        Assert.Single(comments);
        Assert.Equal("Great video!", comments[0].Body);
    }

    [Fact]
    public async Task GetAllActiveAsync_SortByViews()
    {
        var result = await _service.GetAllActiveAsync("Views");
        Assert.Equal(2, result.Count);
        Assert.True(result[0].Views >= result[1].Views);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
