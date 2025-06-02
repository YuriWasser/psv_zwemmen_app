using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Gebruiker;

public class LogInModel : PageModel
{
    [BindProperty]
    public string Gebruikersnaam { get; set; }

    [BindProperty]
    public string Wachtwoord { get; set; }

    public void OnGet()
    {
        // Alleen de pagina tonen, geen loginlogica hier
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Simpele loginlogica (vervang dit later met echte authenticatie)
        if (Gebruikersnaam == "admin" && Wachtwoord == "wachtwoord")
        {
            return RedirectToPage("/Index"); // Succesvol ingelogd
        }

        // Foutmelding als inloggegevens niet kloppen
        ModelState.AddModelError(string.Empty, "Ongeldige inloggegevens.");
        return Page();
    }
}