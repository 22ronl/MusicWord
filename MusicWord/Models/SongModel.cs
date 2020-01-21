using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    /// <summary>
    ///  Class <c>SongModel</c>implemnts ICategory, and holds the songs informaion
    /// exatrct from data base.
    /// </summary>
    class SongModel : ICategory
    {
        public SongModel(Int64 id, string name, Int64 artistId, Int64 albumId)
        {
            this.Id = id;
            this.Name = name;
            this.AlbumId = albumId;
            this.ArtistId = artistId;
        }

        public Int64 Id { get; set; }

        public string Name { get; set; }

        public Int64 ArtistId { get; set; }

        public Int64 AlbumId { get; set; }
    }
}
