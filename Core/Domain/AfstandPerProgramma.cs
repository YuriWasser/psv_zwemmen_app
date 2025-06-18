namespace Core.Domain;

public class AfstandPerProgramma
{
    public int ProgrammaId { get; set; }
    public int AfstandId { get; set; }
    public int Volgorde { get; set; }
    
    public AfstandPerProgramma(int programmaId, int afstandId, int volgorde)
    {
        ProgrammaId = programmaId;
        AfstandId = afstandId;
        Volgorde = volgorde;
    }
}