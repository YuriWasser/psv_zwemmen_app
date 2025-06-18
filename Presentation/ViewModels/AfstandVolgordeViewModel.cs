namespace Presentation.ViewModels;

public class AfstandVolgordeViewModel
{
    public int AfstandId { get; set; }
    public string AfstandNaam { get; set; }
    public bool Geselecteerd { get; set; }
    public int? Volgorde { get; set; }
    
    public AfstandVolgordeViewModel(int afstandId, string afstandNaam, bool geselecteerd, int? volgorde)
    {
        AfstandId = afstandId;
        AfstandNaam = afstandNaam;
        Geselecteerd = geselecteerd;
        Volgorde = volgorde;
    }

    public AfstandVolgordeViewModel()
    {
        
    }
}