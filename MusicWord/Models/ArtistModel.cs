using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    /// <summary>
    /// Class <c>ArtistModel</c> implemnts ICategory, and holds the artists informaion
    /// exatrct from data base.
    /// </summary>
    class ArtistModel : ICategory
    {
        public ArtistModel(Int64 id, string name, string country)
        {
            this.Id = id;
            this.Name = name;
            this.Country = country;
        }

        public Int64 Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }
    }
}
