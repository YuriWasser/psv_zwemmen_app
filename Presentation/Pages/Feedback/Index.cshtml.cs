using Core.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Feedback;

public class IndexModel : PageModel
{
    private readonly FeedbackService _feedbackService;

    public IndexModel(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    public List<FeedbackViewModel> Feedbacks { get; set; } = new();

    public void OnGet()
    {
        // Haal gebruikerId uit claims
        var gebruikerIdClaim = User.FindFirst("GebruikerId")?.Value;
        if (int.TryParse(gebruikerIdClaim, out int gebruikerId))
        {
            var feedbacks = _feedbackService.GetByZwemmerId(gebruikerId);
            Feedbacks = new List<FeedbackViewModel>();
            foreach (var f in feedbacks)
            {
                Feedbacks.Add(new FeedbackViewModel(
                    f.Id,
                    f.GebruikerId,
                    f.ProgrammaId,
                    f.FeedbackText
                ));
            }
        }
        else
        {
            Feedbacks = new List<FeedbackViewModel>();
        }
    }
}