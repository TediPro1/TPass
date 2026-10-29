using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPass_Core.Interfaces;
using TPass.Models.Credentials;
using TPass_Data.Entities;

namespace TPass.Controllers;

[Authorize]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class CredentialsController(ICredentialsService credentials) : Controller
{
    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var items = await credentials.GetAllAsync(CurrentUserId);
        return View(items.Select(c => new CredentialsIndex
        {
            Id = c.Id, Website = c.Website, Username = c.Username
        }));
    }

    public async Task<IActionResult> Details(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var credential = await credentials.GetByIdAsync(CurrentUserId, id);
        if (credential is null) return NotFound();
        return View(new CredentialsDetails
        {
            Id = credential.Id,
            Website = credential.Website,
            Username = credential.Username,
            Password = credential.Password,
            CreatedOn = credential.CreatedOn,
            Tags = credential.CredentialTags.OrderBy(ct => ct.Tag.Name).Select(ct => ct.Tag.Name).ToList()
        });
    }

    public IActionResult Create() => View(new CredentialsCreate());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CredentialsCreate model)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        if (!ModelState.IsValid) return View(model);
        await credentials.CreateAsync(CurrentUserId, new Credential
        {
            Website = model.Website, Username = model.Username, Password = model.Password
        });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var credential = await credentials.GetByIdAsync(CurrentUserId, id);
        if (credential is null) return NotFound();
        return View(new CredentialsEdit
        {
            Id = credential.Id, Website = credential.Website,
            Username = credential.Username, Password = credential.Password
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CredentialsEdit model)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            if (!await credentials.ExistsAsync(CurrentUserId, id)) return NotFound();
            return View(model);
        }
        if (!await credentials.UpdateAsync(CurrentUserId, new Credential
        {
            Id = model.Id, Website = model.Website,
            Username = model.Username, Password = model.Password
        })) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var credential = await credentials.GetByIdAsync(CurrentUserId, id);
        if (credential is null) return NotFound();
        return View(new CredentialsDelete
        {
            Id = credential.Id, Website = credential.Website, Username = credential.Username
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        if (!await credentials.DeleteAsync(CurrentUserId, id)) return NotFound();
        return RedirectToAction(nameof(Index));
    }
}
