using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Service;
using Presentation.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Presentation.Pages.Trainingen;

public class IndexModel : PageModel
{
    private readonly TrainingService _trainingService;

    public IndexModel(TrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    // Lijst van trainingen die je in de view toont
    public List<TrainingViewModel> Trainingen { get; set; } = new();

    public void OnGet()
    {
        // Haal gebruikerId uit claims (indien aanwezig)
        var gebruikerIdClaim = User.FindFirst("GebruikerId")?.Value;
        int gebruikerId = 0;
        if (!string.IsNullOrEmpty(gebruikerIdClaim))
            int.TryParse(gebruikerIdClaim, out gebruikerId);

        // Haal actieve trainingen op via de service
        var trainingen = _trainingService.GetActieveTrainingen(gebruikerId);

        // Zet om naar ViewModels
        Trainingen = new List<TrainingViewModel>();
        foreach (var t in trainingen)
        {
            Trainingen.Add(new TrainingViewModel(
                t.Id,
                t.ZwembadId,
                t.Datum,
                t.Tijd
            ));
        }
    }
}