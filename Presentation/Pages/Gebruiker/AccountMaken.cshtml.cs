using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Pages.Gebruiker
{
    public class AccountMaken : PageModel
    {
        private readonly GebruikerService _gebruikerService;

        public AccountMaken(GebruikerService gebruikerService)
        {
            _gebruikerService = gebruikerService;
        }

        [BindProperty, Required]
        public string Voornaam { get; set; }

        [BindProperty, Required]
        public string Achternaam { get; set; }

        [BindProperty, Required]
        public string Gebruikersnaam { get; set; }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Wachtwoord { get; set; }

        [BindProperty, Required]
        public string Functie { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            //     return Page();

            try
            {
                _gebruikerService.Add(
                    0, // ID is auto increment
                    Gebruikersnaam,
                    Wachtwoord,
                    Email,
                    Voornaam,
                    Achternaam,
                    Functie
                );

                return RedirectToPage("/Gebruiker/LogIn");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Fout bij registreren: " + ex.Message);
                return Page();
            }
        }
    }
}