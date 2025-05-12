using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;

namespace Presentation.Pages.Programma;

public class ProgrammaModel : PageModel
{
    private readonly ProgrammaService _programmaService;
    private readonly AfstandService _afstandService;
    
    public ProgrammaModel(ProgrammaService programmaService, AfstandService afstandService)
    {
        _afstandService = afstandService;
        _programmaService = programmaService;
    }
    public ProgrammaViewModel Programma { get; set; }
    public List<AfstandViewModel> Afstanden { get; set; } = new List<AfstandViewModel>();
    public IActionResult OnGet(int id)
    {
        Core.Domain.Programma prog = _programmaService.GetById(id);
        var afstanden = _afstandService.GetByProgrammaId(prog.Id);
        foreach(var afstand in afstanden)
        {
            Afstanden.Add(new AfstandViewModel(afstand.Id, afstand.Meters, afstand.Beschrijving));
        }
        Programma = new ProgrammaViewModel
        (
            prog.CompetitieId,
            prog.Omschrijving,
            prog.Datum,
            prog.StartTijd,
            Afstanden
        );
        return Page();
    }
}