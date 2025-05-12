namespace Presentation.ViewModels;

public class CompetitieViewModel
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public DateOnly StartDatum { get; set; }
    public DateOnly EindDatum { get; set; }
    public int ZwembadId { get; set; }
    public string ZwembadAdres { get; set; }
    public int ProgrammaId { get; set; }

    public CompetitieViewModel(int id, string naam, DateOnly startDatum, DateOnly eindDatum,
        int zwembadId)
    {
        Id = id;
        Naam = naam;
        StartDatum = startDatum;
        EindDatum = eindDatum;
        ZwembadId = zwembadId;
    }

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