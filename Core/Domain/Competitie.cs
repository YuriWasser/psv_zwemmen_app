
namespace Core.Domain;
public class Competitie
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public DateOnly StartDatum { get; set; }
    public DateOnly EindDatum { get; set; }
    public int ZwembadId { get; set; }
    public int ProgrammaId { get; set; }

    public Competitie(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId, int programmaId)
    {
        Id = id;
        Naam = naam;
        StartDatum = startDatum;
        EindDatum = eindDatum;
        ZwembadId = zwembadId;
        ProgrammaId = programmaId;  
    }
}