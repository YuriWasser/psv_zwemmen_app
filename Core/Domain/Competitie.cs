namespace Core.Domain;

public class Competitie
{
    public int Id { get; set; }
    public string Naam { get; private set; }
    public DateOnly StartDatum { get; private set; }
    public DateOnly EindDatum { get; private set; }
    public int ZwembadId { get; private set; }
    public int ProgrammaId { get; private set; }

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