// Pages/Resultaat/Index.cshtml.cs
using Core.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Presentation.Pages.Resultaat
{
    public class IndexModel : PageModel
    {
        private readonly ResultaatService _resultaatService;

        public IndexModel(ResultaatService resultaatService)
        {
            _resultaatService = resultaatService;
        }

        public List<ResultaatViewModel> Resultaten { get; set; } = new();

        public void OnGet()
        {
            // Haal gebruikerId uit claims
            var gebruikerIdClaim = User.FindFirst("GebruikerId")?.Value;
            if (int.TryParse(gebruikerIdClaim, out int gebruikerId))
            {
                var resultaten = _resultaatService.GetByGebruikerId(gebruikerId);
                Resultaten = new List<ResultaatViewModel>();
                foreach (var r in resultaten)
                {
                    Resultaten.Add(new ResultaatViewModel(
                        r.Id,
                        r.GebruikerId,
                        r.ProgrammaId,
                        r.AfstandId,
                        r.Tijd,
                        r.Datum
                    ));
                }
            }
            else
            {
                Resultaten = new List<ResultaatViewModel>();
            }
        }
    }
}