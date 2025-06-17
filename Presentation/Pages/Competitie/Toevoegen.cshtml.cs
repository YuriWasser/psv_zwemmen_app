// Presentation/Pages/Competitie/ToevoegenModel.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Service;
using Presentation.ViewModels;

namespace Presentation.Pages.Competitie
{
    [Authorize(Roles = "Trainer")]
    public class ToevoegenModel : PageModel
    {
        private readonly CompetitieService _competitieService;
        private readonly ZwembadService _zwembadService;
        private readonly ProgrammaService _programmaService;
        private readonly AfstandService _afstandService;

        public ToevoegenModel(
            CompetitieService competitieService,
            ZwembadService zwembadService,
            ProgrammaService programmaService,
            AfstandService afstandService)
        {
            _competitieService = competitieService;
            _zwembadService = zwembadService;
            _programmaService = programmaService;
            _afstandService = afstandService;
        }

        [BindProperty] public string Naam { get; set; }
        [BindProperty] public DateOnly StartDatum { get; set; }
        [BindProperty] public DateOnly EindDatum { get; set; }
        [BindProperty] public int ZwembadId { get; set; }

        // Programma velden
        [BindProperty] public string ProgrammaOmschrijving { get; set; }
        [BindProperty] public DateTime ProgrammaDatum { get; set; }
        [BindProperty] public TimeSpan ProgrammaStarttijd { get; set; }

        // Afstanden
        [BindProperty] public List<int> GeselecteerdeAfstandIds { get; set; } = new();
        
        [BindProperty]
        public List<AfstandVolgordeViewModel> Afstanden { get; set; } = new();

        public List<SelectListItem> AfstandenSelectList { get; set; }

        public List<SelectListItem> Zwembaden { get; set; }
        

        public void OnGet()
        {
            Zwembaden = _zwembadService.GetAll()
                .Select(z => new SelectListItem
                {
                    Value = z.Id.ToString(),
                    Text = z.Naam
                })
                .ToList();

            AfstandenSelectList = _afstandService.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Beschrijving}"
                })
                .ToList();
            
            Afstanden = _afstandService.GetAll()
                .Select(a => new AfstandVolgordeViewModel
                {
                    AfstandId = a.Id,
                    AfstandNaam = $"{a.Beschrijving}",
                    Geselecteerd = false,
                    Volgorde = null
                }).ToList();

        }

        public IActionResult OnPost()
        {
            Zwembaden = _zwembadService.GetAll()
                .Select(z => new SelectListItem
                {
                    Value = z.Id.ToString(),
                    Text = z.Naam
                })
                .ToList();

            AfstandenSelectList = _afstandService.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Meters}m {a.Beschrijving}"
                })
                .ToList();
            
            Afstanden = _afstandService.GetAll()
                .Select(a => new AfstandVolgordeViewModel
                {
                    AfstandId = a.Id,
                    AfstandNaam = $"{a.Beschrijving}",
                    Geselecteerd = false,
                    Volgorde = null
                }).ToList();

            if (!ModelState.IsValid)
                return Page();

            // Nieuw programma aanmaken
            var programma = _programmaService.Add(
                0, // id
                0, // competitieId, kan je eventueel later koppelen
                ProgrammaOmschrijving,
                ProgrammaDatum,
                ProgrammaStarttijd
            );

            // Koppel de geselecteerde afstanden aan het programma
            foreach (var afstandId in GeselecteerdeAfstandIds)
            {
                // Je moet hier een service/repository aanroepen om de relatie op te slaan
                // Bijvoorbeeld: _programmaService.AddAfstandToProgramma(programma.Id, afstandId);
            }

            // Competitie aanmaken met het nieuwe programma
            _competitieService.Add(0, Naam, StartDatum, EindDatum, ZwembadId, programma.Id);

            return RedirectToPage("/Competitie/Index");
        }
    }
}
