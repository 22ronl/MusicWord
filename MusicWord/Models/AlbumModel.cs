using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// /
/// </summary>
namespace MusicWord.Models
{
    class AlbumModel : ICategory
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public Int64 ArtistId { get; set; }

        public AlbumModel(Int64 id, string name, int year, Int64 artistId)
        {
            this.Id = id;
            this.Name = name;
            this.Year = year;
            this.ArtistId = artistId;
        }
    }
}
