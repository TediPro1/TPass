using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TPass.ViewModels.Credentials;
using TPass_Data.Data;
using TPass_Data.Entities;

namespace TPass.Controllers;

[Authorize]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class CredentialsController(ApplicationDbContext context) : Controller
{
    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    private IQueryable<Credential> OwnedCredentials => context.Credentials
        .Where(c => CurrentUserId != null && c.UserId == CurrentUserId && !c.IsDeleted);

    public async Task<IActionResult> Index()
    {
        return View(await OwnedCredentials.AsNoTracking().Select(c => new CredentialsIndex
        {
            Id = c.Id, Website = c.Website, Username = c.Username
        }).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var credential = await OwnedCredentials.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        if (credential == null) return NotFound();
        return View(new CredentialsDetails
        {
            Id = credential.Id, Website = credential.Website, Username = credential.Username,
            Password = credential.Password, CreatedOn = credential.CreatedOn
        });
    }

    public IActionResult Create() => View(new CredentialsCreate());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CredentialsCreate model)
    {
        if (CurrentUserId == null) return Challenge();
        if (!ModelState.IsValid) return View(model);
        context.Credentials.Add(new Credential
        {
            Website = model.Website, Username = model.Username,
            Password = model.Password, UserId = CurrentUserId,
            CreatedOn = DateTime.UtcNow, IsDeleted = false
        });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var credential = await OwnedCredentials.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        if (credential == null) return NotFound();
        return View(new CredentialsEdit
        {
            Id = credential.Id, Website = credential.Website, Username = credential.Username,
            Password = credential.Password
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CredentialsEdit model)
    {
        if (id != model.Id) return NotFound();
        var credential = await OwnedCredentials.SingleOrDefaultAsync(c => c.Id == id);
        if (credential == null) return NotFound();
        if (!ModelState.IsValid) return View(model);
        credential.Website = model.Website;
        credential.Username = model.Username;
        credential.Password = model.Password;
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OwnedCredentials.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var credential = await OwnedCredentials.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        if (credential == null) return NotFound();
        return View(new CredentialsDelete
        {
            Id = credential.Id, Website = credential.Website, Username = credential.Username
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var credential = await OwnedCredentials.SingleOrDefaultAsync(c => c.Id == id);
        if (credential == null) return NotFound();
        credential.IsDeleted = true;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

