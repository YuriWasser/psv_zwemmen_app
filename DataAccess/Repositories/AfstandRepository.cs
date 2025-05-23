using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class AfstandRepository(string connectionString, ILogger<AfstandRepository> logger) : IAfstandRepository
    {
        public List<Afstand> GetAll()
        {
            try
            {
                List<Afstand> afstanden = new List<Afstand>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, meters, beschrijving FROM afstand";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    afstanden.Add(
                        new Afstand(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("meters")),
                            reader.GetString(reader.GetOrdinal("beschrijving"))
                        )
                    );
                }

                return afstanden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching afstanden");
                throw new DatabaseException("Error fetching afstanden", ex);
            }
        }

        public Afstand GetByID(int afstandId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, meters, beschrijving FROM afstand WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", afstandId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Afstand(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("meters")),
                        reader.GetString(reader.GetOrdinal("beschrijving"))
                    );
                }

                throw new AfstandNotFoundException($"Afstand with ID {afstandId} not found");
            }
            catch (AfstandNotFoundException ex)
            {
                logger.LogWarning(ex, "Afstand not found");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching afstand by ID");
                throw new DatabaseException("Error fetching afstand by ID", ex);
            }
        }

        public Afstand Add(Afstand afstand)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO afstand (meters, beschrijving) VALUES (@meters, @beschrijving)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@meters", afstand.Meters);
                command.Parameters.AddWithValue("@beschrijving", afstand.Beschrijving);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Afstand(newId, afstand.Meters, afstand.Beschrijving);
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error adding afstand");
                throw new DatabaseException("Error adding afstand", ex);
            }
        }

        public bool Update(Afstand afstand)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE afstand SET meters = @meters, beschrijving = @beschrijving WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@meters", afstand.Meters);
                command.Parameters.AddWithValue("@beschrijving", afstand.Beschrijving);
                command.Parameters.AddWithValue("@id", afstand.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error updating afstand");
                throw new DatabaseException("Error updating afstand", ex);
            }
        }

        public bool Delete(Afstand afstand)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM afstand WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", afstand.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error deleting afstand");
                throw new DatabaseException("Error deleting afstand", ex);
            }
        }

        public List<Afstand> GetByProgrammaId(int programmaId)
        {
            try
            {
                List<Afstand> afstanden = new List<Afstand>();
        
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
        
                string sql = @"
                    SELECT a.id, a.meters, a.beschrijving
                    FROM afstand_per_programma pa
                    INNER JOIN afstand a ON pa.afstand_id = a.id
                    WHERE pa.programma_id = @programmaId";
        
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@programmaId", programmaId);
        
                using MySqlDataReader reader = command.ExecuteReader();
        
                while (reader.Read())
                {
                    afstanden.Add(
                        new Afstand(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("meters")),
                            reader.GetString(reader.GetOrdinal("beschrijving"))
                        )
                    );
                }
        
                return afstanden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching afstanden by programmaId");
                throw new DatabaseException("Error fetching afstanden by programmaId", ex);
            }
        }
    }
}