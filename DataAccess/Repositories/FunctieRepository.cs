using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class FunctieRepository(string connectionString, ILogger<FunctieRepository> logger) : IFunctieRepository
    {
        public List<Functie> GetAll()
        {
            try
            {
                var functies = new List<Functie>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT code, beschrijving FROM lookup_functie";

                using var command = new MySqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    functies.Add(new Functie(
                        reader.GetString("code"),
                        reader.GetString("beschrijving")
                    ));
                }

                return functies;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van alle functies.");
                throw new DatabaseException("Er is een databasefout opgetreden bij het ophalen van functies.", ex);
            }
        }

        public Functie GetByCode(string code)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT code, beschrijving FROM lookup_functie WHERE code = @code";

                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@code", code);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Functie(
                        reader.GetString("code"),
                        reader.GetString("beschrijving")
                    );
                }

                throw new FunctieNotFoundException($"Functie met code {code} niet gevonden.");
            }
            catch(FunctieNotFoundException)
            {
                logger.LogWarning($"Functie met code {code} niet gevonden.");
                throw; // Hergooi de exceptie
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij het ophalen van functie met code {code}.");
                throw new DatabaseException($"Er is een databasefout opgetreden bij het ophalen van functie met code {code}.", ex);
            }
        }
    }
}