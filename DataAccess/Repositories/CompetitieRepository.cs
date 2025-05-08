using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class CompetitieRepository : ICompetitieRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();
        
        public List<Competitie> GetAll()
        {
            List<Competitie> competities = new List<Competitie>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM competitie";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                competities.Add(
                    new Competitie(
                        (string)reader["naam"],
                        (DateTime)reader["start_datum"],
                        (DateTime)reader["eind_datum"],
                        (int)reader["zwembad_id"]
                    )
                );
            }

            return competities;
        }

        public Competitie GetById(int competitieId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM competitie WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", competitieId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Competitie(
                    (string)reader["naam"],
                    (DateTime)reader["start_datum"],
                    (DateTime)reader["eind_datum"],
                    (int)reader["zwembad_id"]
                );
            }

            return null;
        }

        public int Add(Competitie competitie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO competitie (naam, start_datum, eind_datum, zwembad_id) VALUES (@naam, @start_datum, @eind_datum, @zwembad_id)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@naam", competitie.Naam);
            command.Parameters.AddWithValue("@start_datum", competitie.StartDatum);
            command.Parameters.AddWithValue("@eind_datum", competitie.EindDatum);
            command.Parameters.AddWithValue("@zwembad_id", competitie.ZwembadId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string selectIdSql = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                return newId;
            }

            return 0;
        }

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
