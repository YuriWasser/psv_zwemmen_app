using Core.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                // 🔐 Token opvragen via service
                string jwtToken = _gebruikerService.Login(Gebruikersnaam, Wachtwoord);

                // ✅ Decodeer JWT-token
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                // 🕵️‍♂️ Optioneel: claims loggen voor debug
                foreach (var claim in token.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                }

                // 📋 Claims extraheren
                var claims = token.Claims.Where(c => c.Type != ClaimTypes.Role).ToList();
                var roles = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // 🍪 Cookie aanmaken
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