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
        private SQLServerModel() { }

        public static SQLServerModel Instance()
        {
            if (_instance == null)
            {
                _instance = new SQLServerModel();
            }
            return _instance;
        }

        public static void connect()
        {
            if (_connention == null || _connention.State != System.Data.ConnectionState.Open)
            {
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
            Instance();
            connect();
            MySqlCommand insertCmd = new MySqlCommand(sqlCommand, _connention);
            insertCmd.ExecuteNonQuery();
            _connention.Close();
        }

        public static ICategory getWord(string category)
        {
            
            PlayerModel player = PlayerModel.Instance;
            Instance();
            connect();

            string sqlQuery = $"SELECT * FROM musicword.{category} ORDER BY RAND() LIMIT 1;";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);

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
                        resCategory = new AlbumModel(reader.GetInt64(0),
                            answer, reader.GetInt16(1), reader.GetInt64(3));
                        break;
                    case "songs":
                        answer = reader.GetString(1);
                        resCategory = new SongModel(reader.GetInt64(0),
                            answer, reader.GetInt64(2), reader.GetInt64(3));
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
            return resCategory;
        }

        public static string getClueString(string connectionString, string sqlQuery)
        {
            Instance();
            connect();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);

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

        public static List<TableEntry> getScoreTable()
        {
            PlayerModel player = PlayerModel.Instance;
            string category = player.Category;
            Instance();
            connect();
            int limit = 10;
            MySqlCommand topPalyersCmd = new MySqlCommand($"SELECT Name, Category, Score FROM " +
                $"players WHERE Category='{category}' ORDER BY score Desc Limit " + limit, _connention);
            var reader = topPalyersCmd.ExecuteReader();
            List<TableEntry> scoreTable = new List<TableEntry>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TableEntry entry = new TableEntry();
                    entry.PlayerName = reader.GetString(0);
                    entry.Category = reader.GetString(1);
                    entry.PlayerScore = reader.GetInt32(2);
                    scoreTable.Add(entry);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            _connention.Close();
            return scoreTable;
        }
        public static void closeConnection()
        {
            _connention.Close();
        }

    }
}
