using Core.Service;
using Core.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Presentation.Pages.Gebruiker
{
    public class LogInModel : PageModel
    {
        private readonly GebruikerService _gebruikerService;
        private readonly IConfiguration _config;

        public LogInModel(GebruikerService gebruikerService, IConfiguration config)
        {
            _gebruikerService = gebruikerService;
            _config = config;
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
                string jwtKey = _config["JwtSettings:Key"];
                string jwtIssuer = _config["JwtSettings:Issuer"];

                // Roep service aan, krijg LoginDto terug
                LoginDto loginDto = _gebruikerService.Login(Gebruikersnaam, Wachtwoord, jwtKey, jwtIssuer);
                string jwtToken = loginDto.TokenString;

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                // Log claims (optioneel)
                foreach (var claim in token.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                }

                // Claims extraheren
                var claims = token.Claims.Where(c => c.Type != ClaimTypes.Role).ToList();
                var roles = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name,
                    ClaimTypes.Role
                );
                var principal = new ClaimsPrincipal(identity);

                // Cookie aanmaken
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
                // Optioneel: _logger.LogError(ex, "Inlogfout");
                return Page();
            }
        }
    }
}