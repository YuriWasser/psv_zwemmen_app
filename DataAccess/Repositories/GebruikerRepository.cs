using Core.Domain;
using Core.Interface;
using MySqlConnector;
namespace DataAccess.Repositories
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();
        
        public List<Gebruiker> GetAll()
        {
            List<Gebruiker> gebruikers = new List<Gebruiker>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM gebruiker";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                gebruikers.Add(
                    new Gebruiker(
                        (string)reader["gebruikersnaam"],
                        (string)reader["wachtwoord"],
                        (string)reader["email"],
                        (string)reader["voornaam"],
                        (string)reader["achternaam"],
                        (string)reader["functieCode"]
                    )
                );
            }

            return gebruikers;
        }

        public Gebruiker GetById(int gebruikerId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM gebruiker WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", gebruikerId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Gebruiker(
                    (string)reader["gebruikersnaam"],
                    (string)reader["wachtwoord"],
                    (string)reader["email"],
                    (string)reader["voornaam"],
                    (string)reader["achternaam"],
                    (string)reader["functieCode"]
                );
            }

            return null;
        }

        public int Add(Gebruiker gebruiker)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO gebruiker " +
                         "(gebruikersnaam, wachtwoord, email, voornaam, achternaam, functieCode) " +
                         "VALUES (@gebruikersnaam, @wachtwoord, @email, @voornaam, @achternaam, @functieCode)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikersnaam", gebruiker.Gebruikersnaam);
            command.Parameters.AddWithValue("@wachtwoord", gebruiker.Wachtwoord);
            command.Parameters.AddWithValue("@email", gebruiker.Email);
            command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
            command.Parameters.AddWithValue("@achternaam", gebruiker.Achternaam);
            command.Parameters.AddWithValue("@functieCode", gebruiker.FunctieCode);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string selectId = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar);
                return newId;
            }

            return 0;
        }

        public bool Update(Gebruiker gebruiker)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE gebruiker SET" +
                         "gebruikersnaam = @gebruikersnaam," +
                         "wachtwoord = @wachtwoord," +
                         "email = @email," +
                         "voornaam = @voornaam," +
                         "achternaam = @achternaam," +
                         "functieCode = @fucntieCode" +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikersnaam", gebruiker.Gebruikersnaam);
            command.Parameters.AddWithValue("@wachtwoord", gebruiker.Wachtwoord);
            command.Parameters.AddWithValue("@email", gebruiker.Email);
            command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
            command.Parameters.AddWithValue("@achternaam", gebruiker.Achternaam);
            command.Parameters.AddWithValue("@functieCode", gebruiker.FunctieCode);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Gebruiker gebruiker)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM gebruiker WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", gebruiker.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}