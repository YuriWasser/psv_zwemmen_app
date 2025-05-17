using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;

namespace Presentation.Pages.Competitie;

// Razor Page model voor het tonen van een lijst competities
public class CompetitieModel : PageModel
{
    // Services voor competitie- en zwembadlogica
    private readonly CompetitieService _competitieService;
    private readonly ZwembadService _zwembadService;

    // Constructor met dependency injection van de services
    public CompetitieModel(CompetitieService competitieService, ZwembadService zwembadService)
    {
        _zwembadService = zwembadService;
        _competitieService = competitieService;
    }

    // Lijst van competities die aan de View worden getoond
    public List<CompetitieViewModel> Competities { get; set; } = new List<CompetitieViewModel>();

    // Lijst van zwembaden 
    public List<Zwembad> Zwembaden { get; set; } = new List<Zwembad>();

    // Methode die wordt aangeroepen bij een GET-verzoek naar de pagina
    public IActionResult OnGet()
    {
        List<Core.Domain.Competitie> competitites = _competitieService.GetAll();

        foreach (var competitie in competitites)
        {
            Competities.Add(new CompetitieViewModel
            (
                competitie.Id,
                competitie.Naam,
                competitie.StartDatum,
                competitie.EindDatum,
                competitie.ZwembadId,
                _zwembadService.GetById(competitie.ZwembadId).Adres,
                competitie.ProgrammaId // **Hier toevoegen**
            ));
        }

        return Page();
    }
}