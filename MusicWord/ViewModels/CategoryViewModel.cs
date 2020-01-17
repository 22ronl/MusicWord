using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    class CategoryViewModel :AppScreen 
    {
        private void OnCategory(string category)
        {
            PlayerModel.Instance.Category = category;
            NextScreen();
        }

        public void Songs()
        {
            OnCategory("songs");
        }
        public void Artists()
        {
            OnCategory("artists");
        }
        public void Albums()
        {
            OnCategory("albums");
        }

    }
}
