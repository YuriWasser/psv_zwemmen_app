using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    // Deze class hoort bij de Razor Page 'ProgrammaPage.cshtml'
    public class ProgrammaPageModel : PageModel
    {
        // Inject de service die de business- en datalaag aanroept
        private readonly ProgrammaService _programmaService;

        // Constructor: ProgrammaService wordt via Dependency Injection beschikbaar gemaakt
        public ProgrammaPageModel(ProgrammaService programmaService)
        {
            _programmaService = programmaService;
        }

        // Deze property wordt automatisch gevuld vanuit de querystring (?competitieId=...)
        [BindProperty(SupportsGet = true)]
        public int CompetitieId { get; set; }

        // Opslag voor de lijst met programma-items die bij deze competitie horen
        public IEnumerable<Programma> Programmas { get; set; } = new List<Programma>();

        // Deze methode wordt uitgevoerd bij een HTTP GET-request naar de pagina
        public async Task OnGetAsync()
        {
            // Haal vanuit de service alle programma's op voor de gegeven competitie
            Programmas = await _programmaService.GetProgrammasByCompetitieIdAsync(CompetitieId);
        }
    }
}