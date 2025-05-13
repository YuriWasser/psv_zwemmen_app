using Core.Domain;
using Core.Interface;

namespace Core.Service
{
    public class ProgrammaService
    {
        private readonly IProgrammaRepository _programmaRepository;

        public ProgrammaService(IProgrammaRepository programmaRepository)
        {
            _programmaRepository = programmaRepository;
        }

        public List<Programma> GetAll()
        {
            try
            {
                return _programmaRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public Programma GetById(int id)
        {
            try
            {
                Programma programma = _programmaRepository.GetById(id);
                if (programma != null)
                {
                    return programma;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }

            return null;
        }

        public Programma Add(int id, int competitieId, string omschrijving, DateTime datum, TimeSpan starttijd)
        {
            try
            {
                Programma programma = new Programma(id, competitieId, omschrijving, datum, starttijd);
                programma.Id = _programmaRepository.Add(programma);
                return programma;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public bool Update(Programma programma)
        {
            try
            {
                return _programmaRepository.Update(programma);
            }
            catch
            {
                throw new Exception("error");
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var programma = _programmaRepository.GetById(id);
                return _programmaRepository.Delete(programma);
            }
            catch
            {
                throw new Exception("error");
            }
        }

        public List<Afstand> GetAfstandenByProgramma(int programmaId)
        {
            try
            {
                return _programmaRepository.GetAfstandenByProgramma(programmaId);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}