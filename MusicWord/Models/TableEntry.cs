using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace MusicWord.Models
{
    /// <summary>
    /// <c>TableEntry</c>
    /// a class that represents an entry of the score table
    /// </summary>
    class TableEntry : PropertyChangedBase
    {
        // the actual column values of the entry, which are the following properties
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
        public string Category { get; set; }
    }
}
