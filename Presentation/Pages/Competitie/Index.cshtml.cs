// Presentation/Pages/Competitie/CompetitieModel.cs

using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Collections.Generic;

namespace Presentation.Pages.Competitie;

[Authorize]
public class CompetitieModel : PageModel
{
    private readonly CompetitieService _competitieService;
    private readonly ZwembadService _zwembadService;
    private readonly ProgrammaService _programmaService;
    private readonly AfstandPerProgrammaService _afstandPerProgrammaService;

    public CompetitieModel(
        CompetitieService competitieService,
        ZwembadService zwembadService,
        ProgrammaService programmaService,
        AfstandPerProgrammaService afstandPerProgrammaService)
    {
        _zwembadService = zwembadService;
        _competitieService = competitieService;
        _programmaService = programmaService;
        _afstandPerProgrammaService = afstandPerProgrammaService;
    }

    public List<CompetitieViewModel> Competities { get; set; } = new List<CompetitieViewModel>();

    public IActionResult OnGet()
    {
        List<Core.Domain.Competitie> competitites = _competitieService.GetActieveCompetities()
            .OrderBy(c => c.StartDatum)
            .ToList();

        foreach (var competitie in competitites)
        {
            ProgrammaViewModel? programmaVm = null;
            List<AfstandViewModel> afstandenVm = new();

            if (competitie.ProgrammaId > 0)
            {
                var programma = _programmaService.GetById(competitie.ProgrammaId);
                if (programma != null)
                {
                    // Haal de afstanden op voor dit programma
                    var afstanden = _afstandPerProgrammaService.GetByProgrammaId(programma.Id);
                    foreach (var afstand in afstanden)
                    {
                        afstandenVm.Add(new AfstandViewModel(
                            afstand.Id,
                            afstand.Meters,
                            afstand.Beschrijving
                        ));
                    }

                    programmaVm = new ProgrammaViewModel(
                        programma.Id,
                        programma.CompetitieId,
                        programma.Omschrijving,
                        programma.Datum,
                        programma.StartTijd,
                        afstandenVm
                    );
                }
            }

            Competities.Add(new CompetitieViewModel(
                competitie.Id,
                competitie.Naam,
                competitie.StartDatum,
                competitie.EindDatum,
                competitie.ZwembadId,
                _zwembadService.GetById(competitie.ZwembadId).Adres,
                competitie.ProgrammaId,
                programmaVm,
                afstandenVm
            ));
        }

        return Page();
    }
}
