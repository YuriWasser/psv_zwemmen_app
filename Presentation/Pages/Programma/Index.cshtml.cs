using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Security.Claims;

namespace Presentation.Pages.Programma
{
    public class ProgrammaModel : PageModel
    {
        private readonly ProgrammaService _programmaService;
        private readonly AfstandService _afstandService;
        private readonly WedstrijdInschrijvingService _inschrijvingService;

        public ProgrammaViewModel? Programma { get; set; }

        [BindProperty] public int ProgrammaId { get; set; }

        [BindProperty] public List<int> GeselecteerdeAfstanden { get; set; } = [];

        public ProgrammaModel(
            ProgrammaService programmaService,
            AfstandService afstandService,
            WedstrijdInschrijvingService inschrijvingService)
        {
            _programmaService = programmaService;
            _afstandService = afstandService;
            _inschrijvingService = inschrijvingService;
        }

        [Authorize(Roles = "Zwemmer")]
        public IActionResult OnGet(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Ongeldig programma ID");
            }

            try
            {
                var programma = _programmaService.GetById(id);
                var afstandEntities = _afstandService.GetByProgrammaId(id);

                var afstanden = afstandEntities
                    .Select(a => new AfstandViewModel(a.Id, a.Meters, a.Beschrijving))
                    .ToList();

                List<int> geregistreerdeAfstanden = new List<int>();

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var idClaim = User.FindFirstValue("GebruikerId");
                    if (int.TryParse(idClaim, out int gebruikerId))
                    {
                        geregistreerdeAfstanden =
                            _inschrijvingService.GetAfstandenByGebruikerEnProgramma(gebruikerId, id);
                    }
                }

                Programma = new ProgrammaViewModel(
                    programma.Id,
                    programma.CompetitieId,
                    programma.Omschrijving,
                    programma.Datum,
                    programma.StartTijd,
                    afstanden,
                    geregistreerdeAfstanden
                );

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }


        public IActionResult OnPost()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Forbid();
            }

            // Debug: log alle claims (tijdelijk om te checken welke claims er zijn)
            var allClaims = string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"));
            Console.WriteLine($"Claims: {allClaims}");

            // Probeer user ID op te halen uit de standaard NameIdentifier claim
            var idClaim = User.FindFirstValue("GebruikerId");

            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var gebruikerId))
            {
                Console.WriteLine($"Geen geldige gebruikerId gevonden in claims. idClaim='{idClaim}'");
                return Unauthorized();
            }

            // Hier kun je gebruikerId verder gebruiken
            foreach (int afstandId in GeselecteerdeAfstanden)
            {
                if (!_inschrijvingService.Exists(gebruikerId, ProgrammaId, afstandId))
                {
                    var inschrijving = new WedstrijdInschrijving(
                        id: 0, // ID wordt gegenereerd door de database
                        gebruikerId: gebruikerId,
                        programmaId: ProgrammaId,
                        afstandId: afstandId,
                        inschrijfDatum: DateTime.Now
                    );

                    _inschrijvingService.Add(inschrijving);
                }
            }

            TempData["Success"] = "Je bent succesvol ingeschreven voor de geselecteerde afstanden.";
            return RedirectToPage(new { id = ProgrammaId });
        }
    }
}