using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    class CluesModel
    {
        private SortedDictionary<string, List<string>> _queries_map = new SortedDictionary<string, List<string>>();
        private ICategory _icategory;
        private HashSet<string> usedClues;
        public CluesModel(ICategory icategory)
        {
            init_queries_map();
            _icategory = icategory;
            usedClues = new HashSet<string>();
        }

        private void init_queries_map()
        {
            //atrist clues
            _queries_map.Add("artists", new List<string>());
            _queries_map["artists"].Add("SELECT name FROM songs WHERE {cur_artist_id} = songs.artist ORDER BY rand() LIMIT 1;");
            _queries_map["artists"].Add("SELECT birthday FROM artists WHERE {cur_artist_id} = artists.id ORDER BY rand() LIMIT 1;");
            _queries_map["artists"].Add("SELECT country FROM artists WHERE {cur_artist_id} = artists.id ORDER BY rand() LIMIT 1;");
            _queries_map["artists"].Add("SELECT gender FROM artists WHERE {cur_artist_id} = artists.id ORDER BY rand() LIMIT 1;");
            _queries_map["artists"].Add("SELECT name FROM albums WHERE {cur_artist_id} = albums.artist ORDER BY rand() LIMIT 1;");

            //songs clues
            _queries_map.Add("songs", new List<string>());
            _queries_map["songs"].Add("SELECT name FROM albums WHERE {cur_song_album_id} = albums.id ORDER BY rand() LIMIT 1;");
            _queries_map["songs"].Add("SELECT name FROM artists WHERE {cur_song_artist_id} = artists.id ORDER BY rand() LIMIT 1;");
            _queries_map["songs"].Add("SELECT name FROM songs WHERE {cur_song_album_id} = songs.album AND {cur_song_id} != songs.id ORDER BY rand() LIMIT 1;");

            //albums clues
            _queries_map.Add("albums", new List<string>());
            _queries_map["albums"].Add("SELECT name FROM artists WHERE {cur_album_artist_id} = artists.id ORDER BY rand() LIMIT 1;");
            _queries_map["albums"].Add("SELECT year FROM albums WHERE {cur_album_id} = albums.id ORDER BY rand() LIMIT 1;");
            _queries_map["albums"].Add("SELECT name FROM albums WHERE {cur_album_artist_id} = albums.artist AND albums.id != {cur_album_id} ORDER BY rand() LIMIT 1;");
        }

        public string getClue()
        {
            PlayerModel newPlayer = PlayerModel.Instance;
            string category = newPlayer.Category;
            Random rand = new Random();
            int index = rand.Next(_queries_map[category].Count);
            string query = _queries_map[category][index];
            string newQuery = toReplace(category, query);
            string connectionString = "SERVER = localhost; DATABASE=musicword; UID= root; PASSWORD=035342770Rl";
            string query_answer = SQLServerModel.getClueString(connectionString, newQuery);
            if (query_answer == null)
            {
                return getClue();
            }
            string answer = completeClueString(newQuery, query_answer);
            if (usedClues.Contains(answer))
            {
                return getClue();
            }
            usedClues.Add(answer);
            return answer;
        }

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
