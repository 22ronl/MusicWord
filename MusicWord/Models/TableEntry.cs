using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace MusicWord.Models
{
    class TableEntry : PropertyChangedBase
    {

        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
        public string Category { get; set; }
    }
}
