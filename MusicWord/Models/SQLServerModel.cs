using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private MySqlConnection _connention;
        // the singleton instance member
        private static SQLServerModel _instance = null;
        /// <summary>
        /// private constructor
        /// </summary>
        private SQLServerModel() 
        {
            connect();
        }
        
        /// <summary>
        /// the singleton instance property
        /// </summary>
        public static  SQLServerModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLServerModel();
                }
                return _instance;
            }
        }

        /// <summary>
        /// function that makes the connection to the database 
        /// </summary>
        public void connect()
        {
            try
            {
                _connention = new MySqlConnection(Globals.connectionString);
                _connention.Open();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Connection Failed");
            }
        }
        
        

        /// <summary>
        /// sends the query to the database
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void connectSQLServer(string sqlCommand)
        {
            try
            {
                // set the query
                MySqlCommand insertCmd = new MySqlCommand(sqlCommand, _connention);
                // execute the query
                insertCmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                MessageBox.Show("MySqlCommand Error");
            }
        }

        /// <summary>
        /// function to get the word that should be guessed in the game
        /// </summary>
        /// <param name="category"></param>
        /// <returns> Icategory interface that represents a category model whose name will 
        /// be used as a word to guess </returns>
        public ICategory getWord(string category)
        {
            ICategory answer = null;
            while(answer == null)
            {
                answer = getCategoryInstance(category);
            }
            return answer;
        }

        /// <summary>
        /// helper function to get the word that should be guessed in the game
        /// </summary>
        /// <param name="category"></param>
        /// <returns> Icategory interface that represents a category model whose name will 
        /// be used as a word to guess
        /// </returns>
        public  ICategory getCategoryInstance(string category)
        {
            // the player instace
            PlayerModel player = PlayerModel.Instance;

            // the query to send to the database
            string sqlQuery = $"SELECT * FROM musicword.{category} ORDER BY RAND() LIMIT 1;";

            MySqlDataReader reader = null;
            try
            {
                // set the query
                MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);
                // get the object that reads the query output
                reader = queryCmd.ExecuteReader();
                if (reader == null)
                {
                    return getCategoryInstance(category);
                }

            }
            catch (MySqlException)
            {
                MessageBox.Show("MySqlCommand Error");
            }
            
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
                        try
                        {
                            answer = reader.GetString(2);
                            // create a new album model
                            resCategory = new AlbumModel(reader.GetInt64(0),
                                answer, reader.GetInt16(1), reader.GetInt64(3));
                        }
                        catch (SqlNullValueException e)
                        {
                            answer = null;
                        }
                        
                        break;
                    case "songs":
                        try
                        {
                            // in this case the answer - song name is in the second column
                            answer = reader.GetString(1);
                            // create a new album model
                            resCategory = new SongModel(reader.GetInt64(0),
                                answer, reader.GetInt64(2), reader.GetInt64(3));
                        }
                        catch (SqlNullValueException e)
                        {
                            answer = null;
                        }
                       
                        break;
                    case "artists":
                        try
                        {
                            // in this case the answer - artist name is in the second column
                            answer = reader.GetString(1);
                            // create a new artist model
                            resCategory = new ArtistModel(reader.GetInt64(0), answer, reader.GetString(4));
                        }
                        catch (SqlNullValueException e)
                        {
                            answer = null;
                        }
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
                // _connention.Close();
                reader.Close();
                return getCategoryInstance(sqlQuery);
            }
            // close reader and connection
            reader.Close();
            // return the category model
            return resCategory;
        }

        /// <summary>
        /// function to get a clue for the player
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlQuery"></param>
        /// <returns> a clue as a string </returns>
        public string getClueString(string connectionString, string sqlQuery)
        {
            MySqlDataReader reader = null;
            try
            {
                // set the query
                MySqlCommand queryCmd = new MySqlCommand(sqlQuery, _connention);
                // get the object that reads the query output
                reader = queryCmd.ExecuteReader();
                if(reader == null)
                {
                    MessageBox.Show("hara al haolam");
                    return getClueString(connectionString, sqlQuery);
                }

            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }

            string answer = null;
            // if the query has results
            if (reader.HasRows)
            {
                // read the first entry
                reader.Read();
                // if the query result is year - convert the int to string
                if (sqlQuery.Contains("year"))
                {
                    try
                    {
                        answer = reader.GetInt16(0).ToString();
                    }
                    catch (SqlNullValueException e)
                    {
                        answer = null;
                    }
                }
                else
                {
                    try
                    {
                        answer = reader.GetString(0);
                    }
                    catch (SqlNullValueException e)
                    {
                        answer = null;
                    }
                }
            }
            // if the query returned a null result, call this function again
            if (answer == null)
            {
                reader.Close();
                return getClueString(connectionString, sqlQuery);
            }
            reader.Close();
            return answer;
        }

        /// <summary>
        /// function to get the score table as a list of table entries
        /// </summary>
        /// <returns></returns>
        public  List<TableEntry> getScoreTable()
        {
            // get the player singleton
            PlayerModel player = PlayerModel.Instance;
            // the current category
            string category = player.Category;
            // set the "top-limit" value of the table (for example top-5 players)
            int limit = 10;

            MySqlDataReader reader = null;
            try
            {
                // set the query
                MySqlCommand topPalyersCmd = new MySqlCommand($"SELECT Name, Category, Score FROM " +
                    $"players WHERE Category='{category}' ORDER BY score Desc Limit " + limit, _connention);
                // get the object that reads the query output
                reader = topPalyersCmd.ExecuteReader();
                if (reader == null)
                {
                    return getScoreTable();
                }

            }
            catch (MySqlException)
            {
                MessageBox.Show("MySqlCommand Error");
            }
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
            // close reader and return the table 
            reader.Close();
            return scoreTable;
        }
        /// <summary>
        /// function to close the connection
        /// </summary>
        public  void closeConnection()
        {

            _connention.Close();
        }
    }
}
