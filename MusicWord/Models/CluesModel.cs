using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    /// <summary>
    /// Class <c>CluesModel</c> responsable on creating clues queries stock.
    /// and components the string clues in run time.
    /// </summary>
    class CluesModel
    {
        private SortedDictionary<string, List<string>> _queries_map = new SortedDictionary<string, List<string>>();
        private ICategory _icategory;
        private HashSet<string> usedClues;
        /// <summary>method <c>CluesModel</c> constructour, init the queirs stock and used clues set</summary>
        public CluesModel(ICategory icategory)
        {
            init_queries_map();
            _icategory = icategory;
            usedClues = new HashSet<string>();
        }

        /// <summary>method <c>init_queries_map</c>responsable for init the queirs stock, 
        /// the words insine the curly brackets will be replaced in run time</summary>
        private void init_queries_map()
        {
            //atrist clues
            _queries_map.Add("artists", new List<string>());
            _queries_map["artists"].Add("SELECT name FROM songs WHERE {cur_artist_id} = songs.artist ORDER BY rand() LIMIT 1;");
            _queries_map["artists"].Add("SELECT birthday FROM artists WHERE {cur_artist_id} = artists.id LIMIT 1;");
            _queries_map["artists"].Add("SELECT country FROM artists WHERE {cur_artist_id} = artists.id LIMIT 1;");
            _queries_map["artists"].Add("SELECT gender FROM artists WHERE {cur_artist_id} = artists.id LIMIT 1;");
            _queries_map["artists"].Add("SELECT name FROM albums WHERE {cur_artist_id} = albums.artist ORDER BY rand() LIMIT 1;");

            //songs clues
            _queries_map.Add("songs", new List<string>());
            _queries_map["songs"].Add("SELECT name FROM albums WHERE {cur_song_album_id} = albums.id LIMIT 1;");
            _queries_map["songs"].Add("SELECT name FROM artists WHERE {cur_song_artist_id} = artists.id LIMIT 1;");
            _queries_map["songs"].Add("SELECT name FROM songs WHERE {cur_song_album_id} = songs.album AND {cur_song_id} != songs.id ORDER BY rand() LIMIT 1;");

            //albums clues
            _queries_map.Add("albums", new List<string>());
            _queries_map["albums"].Add("SELECT name FROM artists WHERE {cur_album_artist_id} = artists.id LIMIT 1;");
            _queries_map["albums"].Add("SELECT year FROM albums WHERE {cur_album_id} = albums.id LIMIT 1;");
            _queries_map["albums"].Add("SELECT name FROM albums WHERE {cur_album_artist_id} = albums.artist AND albums.id != {cur_album_id} ORDER BY rand() LIMIT 1;");
            _queries_map["albums"].Add("SELECT name FROM songs WHERE {cur_album_id} = songs.album ORDER BY rand() LIMIT 1;");

        }

        /// <summary>method <c>getClue</c> responsiable on componinting the clue the returns the preperd string to show</summary>
        public string getClue()
        {
            PlayerModel newPlayer = PlayerModel.Instance;
            string category = newPlayer.Category;
            Random rand = new Random();
            int index = rand.Next(_queries_map[category].Count);
            string query = _queries_map[category][index];
            string newQuery = toReplace(category, query);
            string connectionString =Globals.connectionString;
            string query_answer = SQLServerModel.getClueString(connectionString, newQuery);
            if (query_answer == null)
            {
                return getClue();
            }
            string answer = completeClueString(newQuery, query_answer);
            // if the clue is already used.
            if (usedClues.Contains(answer))
            {
                return getClue();
            }
            usedClues.Add(answer);
            return answer;
        }

        /// <summary>method <c>completeClueString</c> responsiable to component the return answer from query to relevent string</summary>
        private string completeClueString(string query, string clueAnswer)
        {
            if (query.Contains("name"))
            {
                if (query.Contains("albums.artist AND albums.id"))
                {
                    return "Another album from the same artist is: " + clueAnswer;
                }
                if (query.Contains("songs.artist"))
                {
                    return "Among the artist's songs is: " + clueAnswer;
                }
                if (query.Contains("artists"))
                {
                    return "The artist's name is: " + clueAnswer;
                }
                if (query.Contains("albums.id"))
                {
                    return "The song's album name is: " + clueAnswer;
                }
                if (query.Contains("albums.artist"))
                {
                    return "Among the artist's albums: " + clueAnswer;
                }
                if (query.Contains("songs.album"))
                {
                    return "Another song in the same album is: " + clueAnswer;
                }

            }
            if (query.Contains("country"))
            {
                return "The artist's origin country is: " + clueAnswer;
            }

            if (query.Contains("birthday"))
            {
                return "The artist's birthdate is: " + clueAnswer;
            }

            if (query.Contains("gender"))
            {
                return "The artist's gender is: " + clueAnswer;
            }
            if (query.Contains("year"))
            {
                return "The album's year releas is: " + clueAnswer;
            }
            return null;
        }

        /// <summary>method <c>completeClueString</c> responsiable on replacing the curly brackets with the relevant
        /// information for ICategory instance</summary>
        private string toReplace(string category, string query)
        {
            string newQuery = null;
            switch (category)
            {
                case "albums":
                    AlbumModel album = (AlbumModel)_icategory;
                    newQuery = query.Replace("{cur_album_id}", album.Id.ToString());
                    newQuery = newQuery.Replace("{cur_album_artist_id}", album.ArtistId.ToString());
                    break;
                case "songs":
                    SongModel song = (SongModel)_icategory;
                    newQuery = query.Replace("{cur_song_album_id}", song.AlbumId.ToString());
                    newQuery = newQuery.Replace("{cur_song_id}", song.Id.ToString());
                    if (newQuery == query)
                    {
                        newQuery = query.Replace("{cur_song_artist_id}", song.ArtistId.ToString());
                    }
                    break;
                case "artists":
                    ArtistModel art = (ArtistModel)_icategory;
                    newQuery = query.Replace("{cur_artist_id}", art.Id.ToString());
                    break;
                default:
                    break;
            }
            return newQuery;
        }

    }
}
