using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace MusicWord.Models
{
    /// <summary>
    /// <c>SQLServerModel</c>
    /// a class responsible to the database connection
    /// </summary>
    class SQLServerModel
    {
        // coonection object to the database
        private static MySqlConnection _connention = null;
        // the singleton instance member
        private static SQLServerModel _instance = null;
        /// <summary>
        /// private constructor
        /// </summary>
        private SQLServerModel() { }
        
        /// <summary>
        /// function to get the singleton instance
        /// <returns> the class instance </returns>
        /// </summary>
        public static SQLServerModel Instance()
        {
            if (_instance == null)
            {
                _instance = new SQLServerModel();
            }
            return _instance;
        }

        /// <summary>
        /// function that makes the connection to the database 
        /// </summary>
        public static void connect()
        {
            // if there is no open connection create one
            if (_connention == null || _connention.State != System.Data.ConnectionState.Open)
            {
                if (_connention != null)
                {
                    // close the taken connection and open a new one
                    _connention.Close();
                    _connention = new MySqlConnection(Globals.connectionString);
                    _connention.Open();
                }
                else
                {
                    // open a new connection
                    _connention = new MySqlConnection(Globals.connectionString);
                    _connention.Open();
                }
            }
        }

        /// <summary>
        /// sends the query to the database
        /// </summary>
        /// <param name="sqlCommand"></param>
        public static void connectSQLServer(string sqlCommand)
        {
            // use this class instance
            Instance();
            // connect to the database
            connect();
            // set the query
            MySqlCommand insertCmd = new MySqlCommand(sqlCommand, _connention);
            // execute the query
            insertCmd.ExecuteNonQuery();
            // close the connection
            _connention.Close();
        }

        /// <summary>
        /// function to get the word that should be guessed in the game
        /// </summary>
        /// <param name="category"></param>
        /// <returns> Icategory interface that represents a category model whose name will 
        /// be used as a word to guess
        /// </returns>
        public static ICategory getWord(string category)
        {
            // the player instace
            PlayerModel player = PlayerModel.Instance;
            // use this class instance
            Instance();
            // connect to the database
            connect();

            // the query to send to the database
            string sqlQuery = $"SELECT * FROM musicword.{category} ORDER BY RAND() LIMIT 1;";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            // set the query
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);

            // get the object that reads the query output
            var reader = queryCmd.ExecuteReader();
            string answer = null;
            ICategory resCategory = null;
            // if the query has results
            if (reader.HasRows)
            {
                // read the first entry
                reader.Read();
                // define the return object according to the current player category 
                switch (player.Category)
                {
                    case "albums":
                        // in this case the answer - album name is in the third column 
                        answer = reader.GetString(2);
                        // create a new album model
                        resCategory = new AlbumModel(reader.GetInt64(0),
                            answer, reader.GetInt16(1), reader.GetInt64(3));
                        break;
                    case "songs":
                        // in this case the answer - song name is in the second column
                        answer = reader.GetString(1);
                        // create a new album model
                        resCategory = new SongModel(reader.GetInt64(0),
                            answer, reader.GetInt64(2), reader.GetInt64(3));
                        break;
                    case "artists":
                        // in this case the answer - artist name is in the second column
                        answer = reader.GetString(1);
                        // create a new artist model
                        resCategory = new ArtistModel(reader.GetInt64(0), answer, reader.GetString(4));
                        break;
                    default:
                        break;
                }
            }
            else
            // if there are no entries to read
            {
                Console.WriteLine("No rows found.");
            }
            // if the word was already asked, call this function again
            if (!player.isInSet(answer))
            {
                _connention.Close();
                return getWord(sqlQuery);
            }
            // close readr and connection
            reader.Close();
            _connention.Close();
            // return the category model
            return resCategory;
        }

        /// <summary>
        /// function to get a clue for the player
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlQuery"></param>
        /// <returns> a clue as a string </returns>
        public static string getClueString(string connectionString, string sqlQuery)
        {
            // use this class instance
            Instance();
            // connect to the database
            connect();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            // set the query
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);

            // get the object that reads the query output
            var reader = queryCmd.ExecuteReader();
            string answer = null;
            // if the query has results
            if (reader.HasRows)
            {
                // read the first entry
                reader.Read();
                // if the query result is year - convert the int to string
                if (sqlQuery.Contains("year"))
                {
                    answer = reader.GetInt16(0).ToString();
                }
                else
                {
                    answer = reader.GetString(0);
                }
            }
            // if the query returned a null result, call this function again
            if (answer == null)
            {
                return getClueString(connectionString, sqlQuery);
            }
            // close connection and return answer
            _connention.Close();
            return answer;
        }

        /// <summary>
        /// function to get the score table as a list of table entries
        /// </summary>
        /// <returns></returns>
        public static List<TableEntry> getScoreTable()
        {
            // get the player singleton
            PlayerModel player = PlayerModel.Instance;
            // the current category
            string category = player.Category;
            // use this class instance
            Instance();
            // connect to the database
            connect();
            // set the "top-limit" value of the table (for example top-5 players)
            int limit = 10;
            // set the query
            MySqlCommand topPalyersCmd = new MySqlCommand($"SELECT Name, Category, Score FROM " +
                $"players WHERE Category='{category}' ORDER BY score Desc Limit " + limit, _connention);

            // get the object that reads the query output
            var reader = topPalyersCmd.ExecuteReader();
            // create a new score table as a list of table entries
            List<TableEntry> scoreTable = new List<TableEntry>();

            // if the query has results
            if (reader.HasRows)
            {
                // while there are entries to read
                while (reader.Read())
                {
                    // init the table entry and add it to the list
                    TableEntry entry = new TableEntry();
                    entry.PlayerName = reader.GetString(0);
                    entry.Category = reader.GetString(1);
                    entry.PlayerScore = reader.GetInt32(2);
                    scoreTable.Add(entry);
                }
            }
            // if there are no results
            else
            {
                Console.WriteLine("No rows found.");
            }
            // close reader and connecion and return the table 
            reader.Close();
            _connention.Close();
            return scoreTable;
        }
        /// <summary>
        /// function to close the connection
        /// </summary>
        public static void closeConnection()
        {
            _connention.Close();
        }

    }
}
