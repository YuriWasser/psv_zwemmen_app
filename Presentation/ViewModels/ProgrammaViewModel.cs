namespace Presentation.ViewModels;

public class ProgrammaViewModel
{
    public int Id { get; set; }
    public int CompetitieId { get; set; }
    public string Omschrijving { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan StartTijd { get; set; }
    public List<AfstandViewModel> Afstanden { get; set; } = new List<AfstandViewModel>();
    
    public ProgrammaViewModel(int id, int competitieId, string omschrijving, DateTime datum, TimeSpan startTijd, List<AfstandViewModel> afstanden)
    {
        Id = id;
        CompetitieId = competitieId;
        Omschrijving = omschrijving;
        Datum = datum;
        StartTijd = startTijd;
        Afstanden = afstanden;
    }
}