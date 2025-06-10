namespace Core.Domain
{
    public class Functie
    {
        public string Code { get; private set; }

        public string Beschrijving { get; private set; }

        
        public Functie(string code, string beschrijving)
        {
            Code = code;
            Beschrijving = beschrijving;
        }
    }
}