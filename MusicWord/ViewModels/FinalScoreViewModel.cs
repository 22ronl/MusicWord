using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    class FinalScoreViewModel : AppScreen
    {
        private int _score;
        public FinalScoreViewModel() 
        {
            Score = PlayerModel.Instance.Score;
        }
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyOfPropertyChange(() => Score);
                PlayerModel player = PlayerModel.Instance;
                string query = $"INSERT INTO musicword.players(Name,Category,Score) VALUES('{player.Name}','{player.Category}','{player.Score}');";
                SQLServerModel.connectSQLServer(query);
            }
        }
        public void NextBottum()
        {
            NextScreen();
        }
    }
}

