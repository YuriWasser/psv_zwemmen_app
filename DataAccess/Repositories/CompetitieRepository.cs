using Core.Domain;
using Core.Exceptions;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class CompetitieRepository(string connectionString, ILogger<CompetitieRepository> logger) : ICompetitieRepository
    {
        public List<Competitie> GetAll()
        {
            try
            {
                List<Competitie> competities = new List<Competitie>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, naam, start_datum, eind_datum, zwembad_id FROM competitie";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    competities.Add(
                        new Competitie(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetString(reader.GetOrdinal("naam")),
                            DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_datum"))),
                            DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("eind_datum"))),
                            reader.GetInt32(reader.GetOrdinal("zwembad_id"))
                        )
                    );
                }
                return competities;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching competities");
                throw new DatabaseException("Error fetching competities", ex);
            }
        }

        public Competitie GetById(int competitieId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, naam, start_datum, eind_datum, zwembad_id FROM competitie WHERE id = @id";

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
                        reader.GetInt32(reader.GetOrdinal("zwembad_id"))
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

        public int Add(Competitie competitie)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO competitie (naam, start_datum, eind_datum, zwembad_id) " +
                             "VALUES (@naam, @start_datum, @eind_datum, @zwembad_id)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@naam", competitie.Naam);
                command.Parameters.AddWithValue("@start_datum", competitie.StartDatum);
                command.Parameters.AddWithValue("@eind_datum", competitie.EindDatum);
                command.Parameters.AddWithValue("@zwembad_id", competitie.ZwembadId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return newId;
                }

                return 0;
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
    }
}