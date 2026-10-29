using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPass_Core.Interfaces;
using TPass.Models.Tags;
using TPass_Data.Entities;
namespace TPass.Controllers;
[Authorize]
public class TagsController(ITagsService tags) : Controller
{
 private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
 public async Task<IActionResult> Index()
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var items = await tags.GetAllAsync(UserId);
  return View(items.Select(t => new TagModel { Id = t.Id, Name = t.Name }));
 }
 public async Task<IActionResult> Details(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var tag = await tags.GetByIdAsync(UserId, id);
  return tag == null ? NotFound() : View(new TagModel { Id = tag.Id, Name = tag.Name });
 }
 public IActionResult Create() => View(new TagModel());
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(TagModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (!ModelState.IsValid) return View(model);
  await tags.CreateAsync(UserId, new Tag { Name = model.Name });
  return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Edit(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var tag = await tags.GetByIdAsync(UserId, id);
  return tag == null ? NotFound() : View(new TagModel { Id = tag.Id, Name = tag.Name });
 }
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Edit(int id, TagModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (id != model.Id || await tags.GetByIdAsync(UserId, id) == null) return NotFound();
  if (!ModelState.IsValid) return View(model);
  if (!await tags.UpdateAsync(UserId, new Tag { Id = model.Id, Name = model.Name })) return NotFound();
  return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Delete(int id)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  var tag = await tags.GetByIdAsync(UserId, id);
  return tag == null ? NotFound() : View(new TagModel { Id = tag.Id, Name = tag.Name });
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
  var credential = await tags.GetCredentialWithTagsAsync(UserId, id);
  if (credential == null) return NotFound();
  return View(await BuildCredentialTagsModelAsync(UserId, credential));
 }
 [HttpPost, ValidateAntiForgeryToken]
 public async Task<IActionResult> Assign(int id, CredentialTagsModel model)
 {
  if (string.IsNullOrWhiteSpace(UserId)) return Challenge();
  if (id != model.CredentialId) return NotFound();
  var current = await tags.GetCredentialWithTagsAsync(UserId, id);
  if (current == null) return NotFound();
  if (ModelState.IsValid && await tags.SetCredentialTagsAsync(UserId, id, model.SelectedTagIds))
   return RedirectToAction("Details", "Credentials", new { id });
  ModelState.AddModelError("", "Choose only tags from your list.");
  var refreshed = await BuildCredentialTagsModelAsync(UserId, current);
  model.Website = refreshed.Website;
  model.AvailableTags = refreshed.AvailableTags;
  return View(model);
 }

 private async Task<CredentialTagsModel> BuildCredentialTagsModelAsync(string userId, Credential credential)
 {
  var availableTags = await tags.GetAllAsync(userId);
  return new CredentialTagsModel
  {
   CredentialId = credential.Id,
   Website = credential.Website,
   SelectedTagIds = credential.CredentialTags.Select(ct => ct.TagId).ToList(),
   AvailableTags = availableTags.Select(t => new TagModel { Id = t.Id, Name = t.Name }).ToList()
  };
 }
}
