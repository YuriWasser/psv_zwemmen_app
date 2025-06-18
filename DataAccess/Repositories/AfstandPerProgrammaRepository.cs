using Core.Domain;
using Core.Exceptions;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class AfstandPerProgrammaRepository(string connectionString, ILogger<AfstandPerProgrammaRepository> logger)
        : IAfstandPerProgrammaRepository
    {
        public void Add(int programmaId, int afstandId, int volgorde)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "INSERT INTO afstand_per_programma (programma_id, afstand_id, volgorde) VALUES (@programmaId, @afstandId, @volgorde)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@programmaId", programmaId);
                command.Parameters.AddWithValue("@afstandId", afstandId);
                command.Parameters.AddWithValue("@volgorde", volgorde);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)

                    throw new DatabaseException("No rows affected while adding afstand_per_programma");
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex,
                    "Error adding afstand_per_programma (programmaId={ProgrammaId}, afstandId={AfstandId}, volgorde={Volgorde})",
                    programmaId, afstandId, volgorde);
                throw new DatabaseException("Error adding afstand_per_programma", ex);
            }
        }

        public List<Afstand> GetByProgrammaId(int programmaId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = @"
                              SELECT a.id, a.meters, a.beschrijving
                              FROM afstand_per_programma app
                              JOIN afstand a ON app.afstand_id = a.id
                              WHERE app.programma_id = @programma_id
                              ORDER BY app.volgorde";


                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@programma_id", programmaId);

                using MySqlDataReader reader = command.ExecuteReader();
                
                List<Afstand> afstanden = new List<Afstand>();
                while (reader.Read())
                {
                    afstanden.Add(new Afstand(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("meters")),
                        reader.GetString(reader.GetOrdinal("beschrijving"))
                    ));
                }


                return afstanden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching afstanden for programmaId {ProgrammaId}", programmaId);
                throw new DatabaseException("Error fetching afstanden for programma", ex);
            }
        }
    }
}