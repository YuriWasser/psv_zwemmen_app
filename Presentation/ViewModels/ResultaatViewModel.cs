namespace Presentation.ViewModels;

public class ResultaatViewModel
{
    public int Id { get; set; }
    public int GebruikerId { get; set; }
    public int ProgrammaId { get; set; }
    public int AfstandId { get; set; }
    public TimeSpan Tijd { get; set; }
    public DateTime Datum { get; set; }
    
    public ResultaatViewModel(int id, int gebruikerId, int programmaId, int afstandId, TimeSpan tijd, DateTime datum)
    {
        Id = id;
        GebruikerId = gebruikerId;
        ProgrammaId = programmaId;
        AfstandId = afstandId;
        Tijd = tijd;
        Datum = datum;
    }
}