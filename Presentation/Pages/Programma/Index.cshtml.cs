using Core.Domain;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Programma;

public class ProgrammaModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}