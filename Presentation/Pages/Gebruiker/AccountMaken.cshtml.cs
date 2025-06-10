using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Pages.Gebruiker
{
    public class AccountMaken : PageModel
    {
        private readonly GebruikerService _gebruikerService;
        private readonly FunctieService _functieService;

        public AccountMaken(GebruikerService gebruikerService, FunctieService functieService)
        {
            _gebruikerService = gebruikerService;
            _functieService = functieService;
        }

        [BindProperty, Required] public string Voornaam { get; set; }

        [BindProperty, Required] public string Achternaam { get; set; }

        [BindProperty, Required] public string Gebruikersnaam { get; set; }

        [BindProperty, Required, EmailAddress] public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Wachtwoord { get; set; }

        [BindProperty, Required] public string Functie { get; set; }

        // Hier de SelectList voor dropdown
        public SelectList FunctiesSelectList { get; set; }

        public void OnGet()
        {
            // Functies ophalen uit de service
            var functies = _functieService.GetAll();
            FunctiesSelectList = new SelectList(functies, "Code", "Beschrijving");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var functies = _functieService.GetAll();
                FunctiesSelectList = new SelectList(functies, "Code", "Beschrijving");
                return Page();
            }

            try
            {
                _gebruikerService.Add(
                    0, // ID is auto increment
                    Gebruikersnaam,
                    Wachtwoord, // sla hashed password op!
                    Email,
                    Voornaam,
                    Achternaam,
                    Functie
                );

                return RedirectToPage("/Gebruiker/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Fout bij registreren: " + ex.Message);
                var functies = _functieService.GetAll();
                FunctiesSelectList = new SelectList(functies, "Code", "Beschrijving");
                return Page();
            }
        }
    }
}