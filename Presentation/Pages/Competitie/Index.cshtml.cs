using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;

namespace Presentation.Pages.Competitie;

[Authorize]
public class CompetitieModel : PageModel
{
    private readonly CompetitieService _competitieService;
    private readonly ZwembadService _zwembadService;

    public CompetitieModel(CompetitieService competitieService, ZwembadService zwembadService)
    {
        _zwembadService = zwembadService;
        _competitieService = competitieService;
    }
    public List<CompetitieViewModel> Competities { get; set; } = new List<CompetitieViewModel>();
    public List<Zwembad> Zwembaden { get; set; } = new List<Zwembad>();
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