namespace Core.Domain
{
    public class Functie
    {
        public string Code { get; set; }

        public string Beschrijving { get; set; }

        
        public Functie(string code, string beschrijving)
        {
            Code = code;
            Beschrijving = beschrijving;
        }
    }
}