// Presentation/ViewModels/CompetitieViewModel.cs

namespace Presentation.ViewModels
{
    public class CompetitieViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public DateOnly StartDatum { get; set; }
        public DateOnly EindDatum { get; set; }
        public int ZwembadId { get; set; }
        public string ZwembadAdres { get; set; }
        public int ProgrammaId { get; set; }
        public ProgrammaViewModel? Programma { get; set; }
        public List<AfstandViewModel> Afstanden { get; set; }

        public CompetitieViewModel(int id, string naam, DateOnly startDatum, DateOnly eindDatum,
            int zwembadId, string zwembadAdres, int programmaId,
            ProgrammaViewModel? programma = null, List<AfstandViewModel>? afstanden = null)
        {
            Id = id;
            Naam = naam;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            ZwembadId = zwembadId;
            ZwembadAdres = zwembadAdres;
            ProgrammaId = programmaId;
            Programma = programma;
            Afstanden = afstanden ?? new List<AfstandViewModel>();
        }
    }
}