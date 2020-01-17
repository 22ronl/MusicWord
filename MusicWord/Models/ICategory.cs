using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    interface ICategory
    {
        string Name { get; set; }
        Int64 Id { get; set; }
    }
}
