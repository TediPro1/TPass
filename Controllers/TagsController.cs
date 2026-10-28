using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPass_Core.Interfaces;
using TPass_Core.Models.Tags;
namespace TPass.Controllers;
[Authorize]
public class TagsController(ITagsService tags) : Controller
{
 private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
 public async Task<IActionResult> Index()
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  return View(await tags.GetAllAsync(UserId));
 }
 public async Task<IActionResult> Details(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var model = await tags.GetAsync(UserId, id);
  return model == null ? NotFound() : View(model);
 }
 public IActionResult Create() => View(new TagModel());
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(TagModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (!ModelState.IsValid) return View(model);
  await tags.CreateAsync(UserId, model);
  return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Edit(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var model = await tags.GetAsync(UserId, id);
  return model == null ? NotFound() : View(model);
 }
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Edit(int id, TagModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (id != model.Id || await tags.GetAsync(UserId, id) == null) return NotFound();
  if (!ModelState.IsValid) return View(model);
  if (!await tags.UpdateAsync(UserId, model)) return NotFound();
  return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Delete(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var model = await tags.GetAsync(UserId, id);
  return model == null ? NotFound() : View(model);
 }
 [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
 public async Task<IActionResult> DeleteConfirmed(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (!await tags.DeleteAsync(UserId, id)) return NotFound();
  return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Assign(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var model = await tags.GetCredentialTagsAsync(UserId, id);
  return model == null ? NotFound() : View(model);
 }
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Assign(int id, CredentialTagsModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (id != model.CredentialId) return NotFound();
  var current = await tags.GetCredentialTagsAsync(UserId, id);
  if (current == null) return NotFound();
  if (ModelState.IsValid && await tags.SetCredentialTagsAsync(UserId, id, model.SelectedTagIds))
   return RedirectToAction("Details", "Credentials", new { id });
  ModelState.AddModelError("", "Choose only tags from your list.");
  model.Website = current.Website;
  model.AvailableTags = current.AvailableTags;
  return View(model);
 }
}
