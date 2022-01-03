using System.Text;
using System.Threading.Tasks;

using AYN.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AYN.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ConfirmEmailChangeModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public ConfirmEmailChangeModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
    {
        if (userId == null || email == null || code == null)
        {
            return this.RedirectToPage("/Index");
        }

        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{userId}'.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await this.userManager.ChangeEmailAsync(user, email, code);
        if (!result.Succeeded)
        {
            this.StatusMessage = "Error changing email.";
            return this.Page();
        }

        await this.signInManager.RefreshSignInAsync(user);
        this.StatusMessage = "Thank you for confirming your email change.";

        return this.Page();
    }
}
