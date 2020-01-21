using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    class LevelThreeViewModel : BaseLevelViewModel
    {
		/// <summary>Class <c>LevelThreeViewModel</c>
		/// Create the game calss with needed parameters
		/// using BaseLevelViewModel
		/// </summary>
		public LevelThreeViewModel()
        {
			ICategory category = SQLServerModel.getWord(PlayerModel.Instance.Category);
			_cluesGenrator = new CluesModel(category);
			string word = category.Name;
			_timer = new TimeModel(Globals.levelTime);
			_timer.PropertyChanged += _timer_PropertyChanged;
			int score = PlayerModel.Instance.Score;
			BaseLevelModel game = new BaseLevelModel(word, _timer, Globals.hidddenPercentage, score);
			baseBuilder(game, score, word);
			game.PropertyChanged += _game_PropertyChanged;
			game.GameOver += _game_GameOver;
			game.start();
		}
    }
}
