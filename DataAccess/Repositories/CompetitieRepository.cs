using Core.Domain;
using Core.Exceptions;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class CompetitieRepository(string connectionString, ILogger<CompetitieRepository> logger) : ICompetitieRepository
    {
        public List<Competitie> GetActieveCompetities()
        {
            try
            {
                List<Competitie> competities = new List<Competitie>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, naam, start_datum, eind_datum, zwembad_id, programma_id FROM competitie WHERE eind_datum >= CURDATE()";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    competities.Add(
                        new Competitie(
                            reader.GetInt32("id"),
                            reader.GetString("naam"),
                            DateOnly.FromDateTime(reader.GetDateTime("start_datum")),
                            DateOnly.FromDateTime(reader.GetDateTime("eind_datum")),
                            reader.GetInt32("zwembad_id"),
                            reader.GetInt32("programma_id")
                        )
                    );
                }

                return competities;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching active competities");
                throw new DatabaseException("Error fetching active competities", ex);
            }
        }

        public Competitie GetById(int competitieId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "SELECT id, naam, start_datum, eind_datum, zwembad_id, programma_id FROM competitie WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", competitieId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Competitie(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetString(reader.GetOrdinal("naam")),
                        DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_datum"))),
                        DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("eind_datum"))),
                        reader.GetInt32(reader.GetOrdinal("zwembad_id")),
                        reader.GetInt32(reader.GetOrdinal("programma_id"))
                    );
                }

                throw new CompetitieNotFoundException("Competitie not found");
            }
            catch (CompetitieNotFoundException ex)
            {
                logger.LogError(ex, "Error fetching competitie by id");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching competitie by id");
                throw new DatabaseException("Error fetching competitie by id", ex);
            }
        }

        public Competitie Add(Competitie competitie)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO competitie (naam, start_datum, eind_datum, zwembad_id, programma_id) " +
                             "VALUES (@naam, @start_datum, @eind_datum, @zwembad_id, @programma_id)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@naam", competitie.Naam);
                command.Parameters.AddWithValue("@start_datum", competitie.StartDatum);
                command.Parameters.AddWithValue("@eind_datum", competitie.EindDatum);
                command.Parameters.AddWithValue("@zwembad_id", competitie.ZwembadId);
                command.Parameters.AddWithValue("@programma_id", competitie.ProgrammaId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Competitie(
                        newId,
                        competitie.Naam,
                        competitie.StartDatum,
                        competitie.EindDatum,
                        competitie.ZwembadId,
                        competitie.ProgrammaId
                    );
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error adding competitie");
                throw new DatabaseException("Error adding competitie", ex);
            }
        }

        public bool Update(Competitie competitie)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE competitie SET " +
                             "naam = @naam, " +
                             "start_datum = @start_datum, " +
                             "eind_datum = @eind_datum, " +
                             "zwembad_id = @zwembad_id " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", competitie.Id);
                command.Parameters.AddWithValue("@naam", competitie.Naam);
                command.Parameters.AddWithValue("@start_datum", competitie.StartDatum);
                command.Parameters.AddWithValue("@eind_datum", competitie.EindDatum);
                command.Parameters.AddWithValue("@zwembad_id", competitie.ZwembadId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true;
                }

                throw new CompetitieNotFoundException("Competitie not found");
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error updating competitie");
                throw new DatabaseException("Error updating competitie", ex);
            }
        }

        public bool Delete(Competitie competitie)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM competitie WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", competitie.Id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true;
                }

                throw new CompetitieNotFoundException("Competitie not found");
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error deleting competitie");
                throw new DatabaseException("Error deleting competitie", ex);
            }
        }

        public List<Programma> GetProgrammaVoorCompetitie(int competitieId)
        {
            try
            {
                List<Programma> programmaLijst = new List<Programma>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "SELECT id, competitie_id, omschrijving, datum, start_tijd FROM programma WHERE competitie_id = @competitieId";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@competitieId", competitieId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    programmaLijst.Add(new Programma(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("competitie_id")),
                        reader.GetString(reader.GetOrdinal("omschrijving")),
                        reader.GetDateTime(reader.GetOrdinal("datum")),
                        reader.GetTimeSpan(reader.GetOrdinal("start_tijd"))
                    ));
                }

                return programmaLijst;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching programma for competitie with ID {CompetitieId}", competitieId);
                throw new DatabaseException($"Error fetching programma for competitie with ID {competitieId}", ex);
            }
        }

        public Programma GetProgrammaById(int id)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, competitie_id, omschrijving, datum, start_tijd FROM programma WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Programma(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("competitie_id")),
                        reader.GetString(reader.GetOrdinal("omschrijving")),
                        reader.GetDateTime(reader.GetOrdinal("datum")),
                        reader.GetTimeSpan(reader.GetOrdinal("start_tijd"))
                    );
                }

                throw new Exception("Programma niet gevonden");
            }
            catch (Exception ex) when (ex is not DatabaseException)
            {
                logger.LogError(ex, "Error fetching programma with ID {Id}", id);
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Database error fetching programma with ID {Id}", id);
                throw new DatabaseException($"Database error fetching programma with ID {id}", ex);
            }
        }
    }
}