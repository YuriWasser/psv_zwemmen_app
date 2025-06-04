using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.Pages.Gebruiker
{
    public class LogInModel : PageModel
    {
        private readonly GebruikerService _gebruikerService;

        public LogInModel(GebruikerService gebruikerService)
        {
            _gebruikerService = gebruikerService;
        }

        [BindProperty]
        public string Gebruikersnaam { get; set; }

        [BindProperty]
        public string Wachtwoord { get; set; }

        public void OnGet()
        {
            // Alleen weergave
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                // 🔐 Vraag token op via service
                string jwtToken = _gebruikerService.Login(Gebruikersnaam, Wachtwoord);

                // ✅ Decodeer het token om claims eruit te halen
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var claims = token.Claims.ToList();

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // 💾 Cookie aanmaken
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToPage("/Index");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(string.Empty, "Ongeldige inloggegevens.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Er is een fout opgetreden tijdens het inloggen.");
                return Page();
            }
        }
    }
}