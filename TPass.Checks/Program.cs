using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPass.Controllers;

using TPass.ViewModels.Credentials;
using TPass_Data.Data;
using TPass_Data.Entities;

int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}
var user = new User { UserName = "tester", Email = "test@example.invalid", PasswordHash = "hash" };
Check(((IdentityUser)user).UserName == user.UserName && ((IdentityUser)user).Email == user.Email && ((IdentityUser)user).PasswordHash == user.PasswordHash, "Identity properties diverge");
Check(typeof(CredentialsController).IsDefined(typeof(AuthorizeAttribute), true), "Controller must require authentication");
await using var db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
var controller = new CredentialsController(db)
{
    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext
    {
        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "owner") }, "test"))
    }}
};
Check(await controller.Create(new CredentialsCreate { Website = "site", Username = "name", Password = "secret" }) is RedirectToActionResult, "Create result");
var created = await db.Credentials.SingleAsync();
Check(created.UserId == "owner" && created.Password == "secret", "Create ownership/plaintext storage");
Check(created.CreatedOn.Kind == DateTimeKind.Utc, "UTC creation time");
var details = (CredentialsDetails)((ViewResult)await controller.Details(created.Id)).Model!;
Check(details.Id == created.Id && details.CreatedOn == created.CreatedOn && details.Password == "secret", "Details mapping");
Check(await controller.Edit(created.Id, new CredentialsEdit { Id = created.Id, Website = "updated", Username = "edited", Password = "changed" }) is RedirectToActionResult, "Edit result");
Check(await db.Credentials.CountAsync() == 1 && created.Website == "updated" && created.Password == "changed", "Edit must update not insert");
db.Credentials.Add(new Credential { UserId = "other", Website = "other", Username = "other", Password = "legacy" });
await db.SaveChangesAsync();
var foreignId = (await db.Credentials.SingleAsync(c => c.UserId == "other")).Id;
Check(((IEnumerable<CredentialsIndex>)((ViewResult)await controller.Index()).Model!).Count() == 1, "Index isolation");
Check(await controller.Details(foreignId) is NotFoundResult && await controller.Edit(foreignId) is NotFoundResult && await controller.Delete(foreignId) is NotFoundResult, "Cross-user reads");
Check(await controller.Edit(foreignId, new CredentialsEdit { Id = foreignId }) is NotFoundResult && await controller.DeleteConfirmed(foreignId) is NotFoundResult, "Cross-user writes");
controller.ModelState.AddModelError("Website", "Required");
Check(((ViewResult)await controller.Create(new CredentialsCreate())).Model is CredentialsCreate, "Invalid create model");
Check(((ViewResult)await controller.Edit(created.Id, new CredentialsEdit { Id = created.Id })).Model is CredentialsEdit, "Invalid edit model");
controller.ModelState.Clear();
Check(await controller.Edit(created.Id, new CredentialsEdit { Id = foreignId }) is NotFoundResult, "Route/model mismatch");
Check(((ViewResult)await controller.Delete(created.Id)).Model is CredentialsDelete, "Delete model");
await controller.DeleteConfirmed(created.Id);
Check(created.IsDeleted && await db.Credentials.CountAsync() == 2, "Soft delete");
Check(await controller.Details(created.Id) is NotFoundResult && await controller.Edit(created.Id) is NotFoundResult && await controller.Delete(created.Id) is NotFoundResult, "Deleted reads");
Check(await controller.Edit(created.Id, new CredentialsEdit { Id = created.Id }) is NotFoundResult && await controller.DeleteConfirmed(created.Id) is NotFoundResult, "Deleted writes");
Check(!((IEnumerable<CredentialsIndex>)((ViewResult)await controller.Index()).Model!).Any(), "Deleted list exclusion");
controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
Check(await controller.Create(new CredentialsCreate()) is ChallengeResult, "Missing user ID");
Console.WriteLine($"Passed {checks} regression checks.");

