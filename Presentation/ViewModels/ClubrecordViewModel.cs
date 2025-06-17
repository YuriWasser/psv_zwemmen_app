namespace Presentation.ViewModels;

public class ClubrecordViewModel
{
    public int Id { get; set; }
    public int GebruikerId { get; set; }
    public int AfstandId { get; set; }
    public TimeSpan Record { get; set; }
    public DateTime Datum { get; set; }

    public ClubrecordViewModel(int id, int gebruikerId, int afstandId, TimeSpan record, DateTime datum)
    {
        Id = id;
        GebruikerId = gebruikerId;
        AfstandId = afstandId;
        Record = record;
        Datum = datum;
    }
}