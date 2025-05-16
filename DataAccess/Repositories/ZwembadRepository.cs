using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class ZwembadRepository(string connectionString, ILogger<ZwembadRepository> logger) : IZwembadRepository
    {
        public List<Zwembad> GetAll()
        {
            try
            {
                List<Zwembad> zwembaden = new List<Zwembad>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, naam, adres FROM zwembad";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    zwembaden.Add(
                        new Zwembad(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetString(reader.GetOrdinal("naam")),
                            reader.GetString(reader.GetOrdinal("adres"))
                        )
                    );
                }

                return zwembaden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching zwembaden");
                throw new DatabaseException("Error fetching zwembaden", ex);
            }
        }

        public Zwembad GetById(int zwembadId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, naam, adres FROM zwembad WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", zwembadId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Zwembad(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetString(reader.GetOrdinal("naam")),
                        reader.GetString(reader.GetOrdinal("adres"))
                    );
                }

                throw new ZwembadNotFoundException($"Zwembad with ID {zwembadId} not found");
            }
            catch (ZwembadNotFoundException ex)
            {
                logger.LogError(ex, "Error fetching zwembad by ID");
                throw new DatabaseException("Error fetching zwembad by ID", ex);
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching zwembad by ID");
                throw new DatabaseException("Error fetching zwembad by ID", ex);
            }
        }

        public int Add(Zwembad zwembad)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO zwembad (naam, adres) VALUES (@naam, @adres)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@naam", zwembad.Naam);
                command.Parameters.AddWithValue("@adres", zwembad.Adres);

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
                logger.LogError(ex, "Error adding zwembad");
                throw new DatabaseException("Error adding zwembad", ex);
            }
        }

        public bool Update(Zwembad zwembad)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE zwembad SET naam = @naam, adres = @adres WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@naam", zwembad.Naam);
                command.Parameters.AddWithValue("@adres", zwembad.Adres);
                command.Parameters.AddWithValue("@id", zwembad.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error updating zwembad");
                throw new DatabaseException("Error updating zwembad", ex);
            }
        }

        public bool Delete(Zwembad zwembad)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM zwembad WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", zwembad.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error deleting zwembad");
                throw new DatabaseException("Error deleting zwembad", ex);
            }
        }
    }
}