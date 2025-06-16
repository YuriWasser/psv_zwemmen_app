using Core.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Clubrecord;

public class IndexModel : PageModel
{
    private readonly ClubrecordService _clubrecordService;

    public IndexModel(ClubrecordService clubrecordService)
    {
        _clubrecordService = clubrecordService;
        
    }
    public List<ClubrecordViewModel> Clubrecords { get; set; } = new();

    public void OnGet()
    {
        var records = _clubrecordService.GetAll();
        Clubrecords = new List<ClubrecordViewModel>();
        foreach (var r in records)
        {
            Clubrecords.Add(new ClubrecordViewModel(
                r.Id,
                r.GebruikerId,
                r.AfstandId,
                r.Tijd,
                r.Datum
            ));
        }
    }

}