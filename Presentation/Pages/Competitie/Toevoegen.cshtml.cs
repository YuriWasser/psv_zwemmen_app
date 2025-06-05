using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Service;
using Core.Domain;

namespace Presentation.Pages.Competitie
{
    [Authorize(Roles = "TrainOnly")]
    public class ToevoegenModel : PageModel
    {
        private readonly CompetitieService _competitieService;

        public ToevoegenModel(CompetitieService competitieService)
        {
            _competitieService = competitieService;
        }

        [BindProperty]
        public string Naam { get; set; }

        [BindProperty]
        public DateOnly StartDatum { get; set; }

        [BindProperty]
        public DateOnly EindDatum { get; set; }

        [BindProperty]
        public int ZwembadId { get; set; }

        [BindProperty]
        public int ProgrammaId { get; set; }

        public void OnGet()
        {
            // Eventuele voorinvulling of ophalen dropdowns kan hier
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _competitieService.Add(0, Naam, StartDatum, EindDatum, ZwembadId, ProgrammaId);

            return RedirectToPage("/Competitie/Index"); // pas aan naar je overzichtspagina
        }
    }
}