using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    // Repository die communicatie met de database regelt voor Competitie-objecten
    public class CompetitieRepository : ICompetitieRepository
    {
        // Klasse die de databaseconnectie levert
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        // Haal alle competities op uit de database
        public List<Competitie> GetAll()
        {
            List<Competitie> competities = new List<Competitie>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM competitie";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            // Lees alle records en zet ze om in Competitie-objecten
            while (reader.Read())
            {
                competities.Add(
                    new Competitie(
                        (int)reader["id"],
                        (string)reader["naam"],
                        DateOnly.FromDateTime(Convert.ToDateTime(reader["start_datum"])),
                        DateOnly.FromDateTime(Convert.ToDateTime(reader["eind_datum"])),
                        (int)reader["zwembad_id"]
                    )
                );
            }

            return competities;
        }

        // Haal één competitie op via het ID
        public Competitie GetById(int competitieId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM competitie WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", competitieId);

            using MySqlDataReader reader = command.ExecuteReader();

            // Return het object als er een record gevonden wordt
            if (reader.Read())
            {
                return new Competitie(
                    (int)reader["id"],
                    (string)reader["naam"],
                    DateOnly.FromDateTime(Convert.ToDateTime(reader["start_datum"])),
                    DateOnly.FromDateTime(Convert.ToDateTime(reader["eind_datum"])),
                    (int)reader["zwembad_id"]
                );
            }

            return null;
        }

        // Voeg een nieuwe competitie toe aan de database
        public int Add(Competitie competitie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO competitie (naam, start_datum, eind_datum, zwembad_id) " +
                         "VALUES (@naam, @start_datum, @eind_datum, @zwembad_id)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@naam", competitie.Naam);
            command.Parameters.AddWithValue("@start_datum", competitie.StartDatum);
            command.Parameters.AddWithValue("@eind_datum", competitie.EindDatum);
            command.Parameters.AddWithValue("@zwembad_id", competitie.ZwembadId);

            int rowsAffected = command.ExecuteNonQuery();

            // Als het invoegen is gelukt, haal het gegenereerde ID op
            if (rowsAffected > 0)
            {
                string selectIdSql = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                return newId;
            }

            return 0;
        }

        // Update een bestaande competitie in de database
        public bool Update(Competitie competitie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
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

            return rowsAffected > 0;
        }

        // Verwijder een competitie uit de database
        public bool Delete(Competitie competitie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM competitie WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", competitie.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}