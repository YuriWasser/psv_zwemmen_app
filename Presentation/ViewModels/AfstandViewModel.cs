namespace Presentation.ViewModels;

public class AfstandViewModel
{
    public int Id { get; set; }
    public int Meters { get; set; }
    public string Beschrijving { get; set; }

    public AfstandViewModel(int id, int meters, string beschrijving)
    {
        Id = id;
        Meters = meters;
        Beschrijving = beschrijving;
    }
}