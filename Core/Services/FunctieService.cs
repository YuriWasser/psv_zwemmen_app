using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Core.Service
{
    public class FunctieService
    {
        private readonly IFunctieRepository _functieRepository;
        private readonly ILogger<FunctieService> _logger;

        public FunctieService(IFunctieRepository functieRepository, ILogger<FunctieService> logger)
        {
            _functieRepository = functieRepository;
            _logger = logger;
        }

        public List<Functie> GetAll()
        {
            try
            {
                var functies = _functieRepository.GetAll();
                if (functies == null)
                {
                    throw new NullReferenceException("De repository retourneerde null");
                }
                return functies;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van functies");
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van functies", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij ophalen van functies");
                throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van functies", ex);
            }
        }

        public Functie GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    throw new ArgumentException("Code mag niet null of leeg zijn", nameof(code));
                
                var functie = _functieRepository.GetByCode(code);
                if (functie == null)
                {
                    throw new Exception($"Functie met code '{code}' niet gevonden");
                }
                return functie;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van functie met code {Code}", code);
                throw new DatabaseException($"Er is een fout opgetreden bij het ophalen van functie met code {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij ophalen van functie met code {Code}", code);
                throw new Exception($"Er is een onverwachte fout opgetreden bij het ophalen van functie met code {code}", ex);
            }
        }
    }
}