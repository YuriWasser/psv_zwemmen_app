namespace Presentation.ViewModels;

public class ProgrammaViewModel
{
    public int Id { get; set; }
    public int CompetitieId { get; set; }
    public string Omschrijving { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan StartTijd { get; set; }
    public List<AfstandViewModel> Afstanden { get; set; } = new List<AfstandViewModel>();
    
    public ProgrammaViewModel(int competitieId, string omschrijving, DateTime datum, TimeSpan starttijd, List<AfstandViewModel> afstanden)
    {
        CompetitieId = competitieId;
        Omschrijving = omschrijving;
        Datum = datum;
        StartTijd = starttijd;
        Afstanden = afstanden;
    }
}