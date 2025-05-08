using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Competitie;

public class CompetitieModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}