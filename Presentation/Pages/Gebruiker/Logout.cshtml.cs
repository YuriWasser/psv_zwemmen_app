using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Gebruiker
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            // Verwijder authenticatiecookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Terug naar de homepage of login
            return RedirectToPage("/Index");
        }
    }
}