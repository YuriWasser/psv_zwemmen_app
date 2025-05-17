using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;

namespace Presentation.Pages.Programma
{
    public class ProgrammaModel : PageModel
    {
        private readonly ProgrammaService _programmaService;
        private readonly AfstandService _afstandService;

        public ProgrammaViewModel? Programma { get; set; }

        public ProgrammaModel(ProgrammaService programmaService, AfstandService afstandService)
        {
            _programmaService = programmaService;
            _afstandService = afstandService;
        }

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

                Programma = new ProgrammaViewModel(
                    programma.Id,
                    programma.CompetitieId,
                    programma.Omschrijving,
                    programma.Datum,
                    programma.StartTijd,
                    afstanden
                );

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}