using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Data;
using VisualVid.Web.Models;

namespace VisualVid.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> Members()
    {
        var members = await _db.Members
            .Include(m => m.User)
            .Include(m => m.Country)
            .OrderByDescending(m => m.User!.UserName)
            .ToListAsync();
        return View(members);
    }

    public async Task<IActionResult> Videos()
    {
        var videos = await _db.Videos
            .Include(v => v.User)
            .Include(v => v.Category)
            .OrderByDescending(v => v.DateAdded)
            .ToListAsync();
        return View(videos);
    }

    [HttpGet]
    public IActionResult Query() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Query(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            ViewBag.Error = "Query cannot be empty.";
            return View();
        }

        // Strict SQL injection prevention: only allow SELECT, block dangerous keywords
        var trimmed = sql.TrimStart();
        if (!trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Error = "Only SELECT queries are allowed.";
            return View();
        }

        // Block statements that could modify data even within a SELECT
        string[] blockedKeywords = [
            "INSERT", "UPDATE", "DELETE", "DROP", "ALTER", "CREATE", "TRUNCATE",
            "EXEC", "EXECUTE", "xp_", "sp_", "GRANT", "REVOKE", "DENY",
            "SHUTDOWN", "BACKUP", "RESTORE", "OPENROWSET", "OPENDATASOURCE",
            "BULK", "INTO", "--", "/*", ";"
        ];

        foreach (var keyword in blockedKeywords)
        {
            if (sql.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = $"Query contains disallowed keyword: {keyword}";
                return View();
            }
        }

        try
        {
            var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;

            await using var reader = await command.ExecuteReaderAsync();
            var results = new List<Dictionary<string, object?>>();
            var columns = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i));

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[columns[i]] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                results.Add(row);
            }

            ViewBag.Columns = columns;
            ViewBag.Results = results;
            ViewBag.Sql = sql;
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.Sql = sql;
        }

        return View();
    }
}
