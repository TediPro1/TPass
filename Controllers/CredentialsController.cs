using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPass_Core.Interfaces;
using TPass_Core.Models.Credentials;

namespace TPass.Controllers;

[Authorize]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class CredentialsController(ICredentialsService credentials) : Controller
{
    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        return View(await credentials.GetAllAsync(CurrentUserId));
    }

    public async Task<IActionResult> Details(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var model = await credentials.GetDetailsAsync(CurrentUserId, id);
        return model == null ? NotFound() : View(model);
    }

    public IActionResult Create() => View(new CredentialsCreate());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CredentialsCreate model)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        if (!ModelState.IsValid) return View(model);
        await credentials.CreateAsync(CurrentUserId, model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var model = await credentials.GetEditAsync(CurrentUserId, id);
        return model == null ? NotFound() : View(model);
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
        if (!await credentials.UpdateAsync(CurrentUserId, model)) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        var model = await credentials.GetDeleteAsync(CurrentUserId, id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId)) return Challenge();
        if (!await credentials.DeleteAsync(CurrentUserId, id)) return NotFound();
        return RedirectToAction(nameof(Index));
    }
}
