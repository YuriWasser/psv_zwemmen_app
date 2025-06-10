using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class ClubrecordRepository(string connectionString, ILogger<ClubrecordRepository> logger)
        : IClubrecordRepository
    {
        public List<Clubrecord> GetAll()
        {
            try
            {
                List<Clubrecord> clubrecords = new List<Clubrecord>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, afstandId, tijd, datum FROM clubrecord";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clubrecords.Add(
                        new Clubrecord(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                            reader.GetInt32(reader.GetOrdinal("afstandId")),
                            reader.GetTimeSpan(reader.GetOrdinal("tijd")),
                            reader.GetDateTime(reader.GetOrdinal("datum"))
                        )
                    );
                }

                return clubrecords;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error retrieving clubrecords");
                throw new DatabaseException("Could not retrieve clubrecords", ex);
            }
        }

        public Clubrecord GetByID(int clubrecordId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, afstandId, tijd, datum FROM clubrecord WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", clubrecordId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Clubrecord(
                        (int)reader["id"],
                        (int)reader["gebruikerId"],
                        (int)reader["afstandId"],
                        (TimeSpan)reader["tijd"],
                        (DateTime)reader["datum"]
                    );
                }

                throw new ClubrecordNotFoundException("Clubrecord not found with the provided ID.");
            }
            catch (ClubrecordNotFoundException ex)
            {
                logger.LogWarning("Error fetching clubrecord by Id");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error retrieving clubrecord by ID");
                throw new DatabaseException("Could not retrieve clubrecord by ID", ex);
            }
        }

        public Clubrecord Add(Clubrecord clubrecord)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "INSERT INTO clubrecord (gebruikerId, afstandId, tijd, datum) VALUES (@gebruikerId, @afstandId, @tijd, @datum)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", clubrecord.GebruikerId);
                command.Parameters.AddWithValue("@afstandId", clubrecord.AfstandId);
                command.Parameters.AddWithValue("@tijd", clubrecord.Tijd);
                command.Parameters.AddWithValue("@datum", clubrecord.Datum);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Clubrecord(
                        newId,
                        clubrecord.GebruikerId,
                        clubrecord.AfstandId,
                        clubrecord.Tijd,
                        clubrecord.Datum
                    );
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error adding clubrecord");
                throw new DatabaseException("Could not add clubrecord", ex);
            }
        }

        public bool Update(Clubrecord clubrecord)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE clubrecord SET " +
                             "gebruikerId = @gebruikerId, " +
                             "afstandId = @afstandId, " +
                             "tijd = @tijd, " +
                             "datum = @datum " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", clubrecord.GebruikerId);
                command.Parameters.AddWithValue("@afstandId", clubrecord.AfstandId);
                command.Parameters.AddWithValue("@tijd", clubrecord.Tijd);
                command.Parameters.AddWithValue("@datum", clubrecord.Datum);
                command.Parameters.AddWithValue("@id", clubrecord.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Error updating clubrecord with ID {clubrecord.Id}");
                throw new DatabaseException($"Could not update clubrecord with ID {clubrecord.Id}", ex);
            }
        }

        public bool Delete(Clubrecord clubrecord)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM clubrecord WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", clubrecord.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Error deleting clubrecord with ID {clubrecord.Id}");
                throw new DatabaseException($"Could not delete clubrecord with ID {clubrecord.Id}", ex);
            }
        }
    }
}