using MySqlConnector;

namespace DataAccess;

public class DatabaseConnection
{
    private readonly string _connectionString = "Server=localhost;Database=DB_PSV_zwemmen;Uid=yuri;Pwd=2003jul6;";
    
    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}