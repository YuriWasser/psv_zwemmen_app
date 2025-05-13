namespace Presentation.ViewModels;

// ViewModel die gebruikt wordt om een competitie weer te geven in de presentatie-laag (bijv. Razor Pages)
public class CompetitieViewModel
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public DateOnly StartDatum { get; set; }
    public DateOnly EindDatum { get; set; }
    public int ZwembadId { get; set; }
    public string ZwembadAdres { get; set; }
    public int ProgrammaId { get; set; }


    // Constructor zonder zwembadadres
    public CompetitieViewModel(int id, string naam, DateOnly startDatum, DateOnly eindDatum,
        int zwembadId)
    {
        Id = id;
        Naam = naam;
        StartDatum = startDatum;
        EindDatum = eindDatum;
        ZwembadId = zwembadId;
    }

    // Constructor met zwembadadres
    public CompetitieViewModel(int id, string naam, DateOnly startDatum, DateOnly eindDatum,
        int zwembadId, string zwembadAdres)
    {
        Id = id;
        Naam = naam;
        StartDatum = startDatum;
        EindDatum = eindDatum;
        ZwembadId = zwembadId;
        ZwembadAdres = zwembadAdres;
    }
}