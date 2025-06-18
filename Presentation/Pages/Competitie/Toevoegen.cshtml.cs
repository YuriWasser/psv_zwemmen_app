// Presentation/Pages/Competitie/ToevoegenModel.cs

using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Service;
using Presentation.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Pages.Competitie
{
    [Authorize(Roles = "Trainer")]
    public class ToevoegenModel : PageModel
    {
        private readonly CompetitieService _competitieService;
        private readonly ZwembadService _zwembadService;
        private readonly ProgrammaService _programmaService;
        private readonly AfstandService _afstandService;
        private readonly AfstandPerProgrammaService _afstandPerProgrammaService;

        public ToevoegenModel(
            CompetitieService competitieService,
            ZwembadService zwembadService,
            ProgrammaService programmaService,
            AfstandService afstandService,
            AfstandPerProgrammaService afstandPerProgrammaService)
        {
            _competitieService = competitieService;
            _zwembadService = zwembadService;
            _programmaService = programmaService;
            _afstandService = afstandService;
            _afstandPerProgrammaService = afstandPerProgrammaService;
        }

        [BindProperty] public string Naam { get; set; }
        [BindProperty] public DateOnly StartDatum { get; set; }
        [BindProperty] public DateOnly EindDatum { get; set; }
        [BindProperty] public int ZwembadId { get; set; }

        // Programma velden
        [BindProperty] public string ProgrammaOmschrijving { get; set; }
        [BindProperty] public DateTime ProgrammaDatum { get; set; }
        [BindProperty] public TimeSpan ProgrammaStarttijd { get; set; }

        [BindProperty] public List<AfstandVolgordeViewModel> Afstanden { get; set; } = new();

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

            Afstanden = _afstandService.GetAll()
                .Select(a => new AfstandVolgordeViewModel
                {
                    AfstandId = a.Id,
                    AfstandNaam = $"{a.Beschrijving}",
                    Geselecteerd = false,
                    Volgorde = null
                }).ToList();
            
            if (StartDatum == default)
                StartDatum = DateOnly.FromDateTime(DateTime.Today);
            if (EindDatum == default)
                EindDatum = DateOnly.FromDateTime(DateTime.Today);
            if (ProgrammaDatum == default)
                ProgrammaDatum = DateTime.Today;
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

    if (!ModelState.IsValid)
    {
        foreach (var key in ModelState.Keys)
        {
            var state = ModelState[key];
            foreach (var error in state.Errors)
            {
                Console.WriteLine($"{key}: {error.ErrorMessage}");
            }
        }
        return Page();
    }

    // 1. Verzamel de geselecteerde afstanden en hun volgorde
    var geselecteerdeAfstanden = Afstanden
        .Where(a => a.Geselecteerd && a.Volgorde.HasValue)
        .OrderBy(a => a.Volgorde.Value)
        .ToList();

    // 2. Voeg de competitie toe (programmaId = 0, want programma bestaat nog niet)
    var toegevoegdeCompetitie = _competitieService.Add(
        0,
        Naam,
        StartDatum,
        EindDatum,
        ZwembadId,
        0 // programmaId is nog niet bekend
    );

    // 3. Voeg het programma toe met het juiste competitieId
    var toegevoegdProgramma = _programmaService.Add(
        0, // id
        toegevoegdeCompetitie.Id, // competitieId
        ProgrammaOmschrijving,
        ProgrammaDatum,
        ProgrammaStarttijd
    );

    // 4. (Optioneel) Update de competitie met het juiste programmaId
    var competitieMetProgrammaId = new Core.Domain.Competitie(
        toegevoegdeCompetitie.Id,
        toegevoegdeCompetitie.Naam,
        toegevoegdeCompetitie.StartDatum,
        toegevoegdeCompetitie.EindDatum,
        toegevoegdeCompetitie.ZwembadId,
        toegevoegdProgramma.Id
    );
    _competitieService.Update(competitieMetProgrammaId);

    // 5. Koppel afstanden aan het programma in afstand_per_programma
    foreach (var afstand in geselecteerdeAfstanden)
    {
        _afstandPerProgrammaService.AddAfstandPerProgramma(
            toegevoegdProgramma.Id,
            afstand.AfstandId,
            afstand.Volgorde.Value
        );
    }

    return RedirectToPage("/Competitie/Index");
}

    }
}