using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    /// <summary>
    /// <c>ScoreViewModel</c>
    /// the score viewModel
    /// </summary>
    class ScoreViewModel : AppScreen
    {
        // score member
        private int _score;
        /// <summary>
        /// constructor
        /// </summary>
		public ScoreViewModel()
		{
            // get the current score from the player
            Score = PlayerModel.Instance.Score;
            // set the hidden word string
			HiddenWord = Globals.hiddenWordText + PlayerModel.Instance.lastWord;
		}
        /// <summary>
        /// Score property
        /// </summary>
		public int  Score
		{
			get { return _score; }
			set 
			{
                // set the score member
                _score = value;
                // notify property change
				NotifyOfPropertyChange(() => Score);
			}
		}
        /// <summary>
        /// function that makes the game move to the next window
        /// </summary>
		public void Next()
		{
			NextScreen();
		}

        /// <summary>
        /// HiddenWord proprety
        /// </summary>
		public string HiddenWord { get; set; }

	}
}
