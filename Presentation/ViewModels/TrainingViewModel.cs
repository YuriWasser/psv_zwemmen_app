namespace Presentation.ViewModels;

public class TrainingViewModel
{
    public int Id { get; private set; }
    public int ZwembadId { get; private set; }
    public DateTime Datum { get; private set; }
    public TimeSpan Tijd { get; private set; }

    public TrainingViewModel(int id, int zwembadId, DateTime datum, TimeSpan tijd)
    {
        Id = id;
        ZwembadId = zwembadId;
        Datum = datum;
        Tijd = tijd;
    }
    
}