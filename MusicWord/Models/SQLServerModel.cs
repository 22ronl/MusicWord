using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace MusicWord.Models
{
    class SQLServerModel
    {
        private static MySqlConnection _connention = null;
        private static SQLServerModel _instance = null;
        private SQLServerModel(){}

        public static SQLServerModel Instance()
        {
            if(_instance == null)
            {
                _instance = new SQLServerModel();
            }
            return _instance;
        }

        public static void connect()
        {
            if(_connention == null || _connention.State != System.Data.ConnectionState.Open)
            {
                //MySqlConnection connection = new MySqlConnection(Globals.connectionString);
                //_connention = connection;
                if (_connention != null)
                {
                    _connention.Close();
                    _connention = new MySqlConnection(Globals.connectionString);
                    _connention.Open();
                }
                else
                {
                    _connention = new MySqlConnection(Globals.connectionString);
                    _connention.Open();
                }
            }
        }
        
        public static void connectSQLServer(string sqlCommand)
        {
            //MySqlConnection connection = new MySqlConnection(Globals.connectionString);
            //connection.Open();

            //SQLServerModel server = Instance();

            Instance();
            connect();
            MySqlCommand insertCmd = new MySqlCommand(sqlCommand, _connention);
            insertCmd.ExecuteNonQuery();
            //connection.Close();
            _connention.Close();
        }

        public static ICategory getWord(string category)
        {
            PlayerModel player = PlayerModel.Instance;
            //SQLServerModel server = Instance();
            Instance();
            connect();
            //MySqlConnection connection = null;

            // connection = new MySqlConnection(Globals.connectionString);
            string sqlQuery = $"SELECT * FROM musicword.{category} ORDER BY RAND() LIMIT 1;";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);
            

            // connection.Open();

            var reader = queryCmd.ExecuteReader();
            string answer = null;
            ICategory resCategory = null;
            if (reader.HasRows)
            {
                reader.Read();
                switch (player.Category)
                {
                    case "albums":
                        answer = reader.GetString(2);
                        resCategory = new AlbumModel(reader.GetInt64(0), answer, reader.GetInt16(1), reader.GetInt64(3));
                        break;
                    case "songs":
                        answer = reader.GetString(1);
                        resCategory = new SongModel(reader.GetInt64(0), answer, reader.GetInt64(2), reader.GetInt64(3));
                        break;
                    case "artists":
                        answer = reader.GetString(1);
                        resCategory = new ArtistModel(reader.GetInt64(0), answer, reader.GetString(4));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            if (!player.isInSet(answer))
            {
                _connention.Close();
                return getWord(sqlQuery);
            }
            reader.Close();
            _connention.Close();
          //  connection.Close();
            return resCategory;
        }

        public static string getClueString(string connectionString, string sqlQuery)
        {
            // MySqlConnection connection = null;
            // connection = new MySqlConnection(connectionString);
            //SQLServerModel server = Instance();
            Instance();
            connect();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);
            
            //connection.Open();

            var reader = queryCmd.ExecuteReader();
            string answer = null;
            if (reader.HasRows)
            {
                reader.Read();
                if (sqlQuery.Contains("year"))
                {
                    answer = reader.GetInt16(0).ToString();
                }
                else
                {
                    answer = reader.GetString(0);
                }
            }
            if (answer == null)
            {
                return getClueString(connectionString, sqlQuery);
            }
            _connention.Close();
            return answer;
        }
        public static void closeConnection()
        {
            _connention.Close();
        }
    }
}
