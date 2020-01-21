using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
using System.Reflection;
using Caliburn.Micro;

namespace MusicWord.ViewModels
{
    class LevelTwoViewModel : LevelOneViewModel
    {
        /// <summary>Class <c>LevelTwoViewModel</c>
        /// Adding restriction on numbers of clues and letter guesses
        /// </summary>
        public class ShowIteam : PropertyChangedBase
        {
            /// <summary>Class <c>ShowIteam</c>
            /// inner class to hide or show view object
            /// </summary>
            private string _toShow;
            public ShowIteam()
            {
                ToShow = "Visable";
            }
            

            public string  ToShow
            {
                get { return _toShow; }
                set 
                { 
                    _toShow = value;
                    NotifyOfPropertyChange(() => ToShow);
                }
            }

            public void hide()
            {
                ToShow = "Hidden";
            }
        }


        private int _lastSymbolIndexClues;
        private int _lastSymbolIndexLetters;
        // the symboals of how much guesses left
        public BindableCollection<ShowIteam> GuessesSymbols { get; set; }
        public BindableCollection<ShowIteam> CluesSymbols { get; set; }
        public LevelTwoViewModel(int numOfClues, int numOfLetters)
        {
            _lastSymbolIndexClues = numOfClues - 1;
            _lastSymbolIndexLetters = numOfLetters - 1;
            CluesSymbols = new BindableCollection<ShowIteam>(intializeCollection(numOfClues));
            GuessesSymbols = new BindableCollection<ShowIteam>(intializeCollection(numOfLetters));
        }
        private List<ShowIteam> intializeCollection(int numOfIteams)
        {
            List<ShowIteam> symbols = new List<ShowIteam>();
            for (int i = 0; i < numOfIteams; i++)
            {
                symbols.Add(new ShowIteam());
            }
            return symbols;
        }
        public override void CheckLetter()
        {
            /// <summary>
            /// Check if the letter is correct and hide one symobl from the screen
            /// </summary>
            bool toEmpty = false;
            if(!String.IsNullOrEmpty(LetterGuess))
            {

                toEmpty = true;
            }
            if (_lastSymbolIndexLetters >= 0 && toEmpty)
            {
    
                _game.EnterGuess(LetterGuess);
                GuessesSymbols[_lastSymbolIndexLetters].hide();
                _lastSymbolIndexLetters -= 1;
                if(_lastSymbolIndexLetters < 0)
                {
                    NoLetters = Globals.noLetters;
                }
            }
            // empty the text box of the letter
            if (toEmpty)
            {
                LetterGuess = "";
            }
        }
        private string _noLetters;

        public string NoLetters
        {
            get { return _noLetters; }
            set 
            { 
                _noLetters = value;
                NotifyOfPropertyChange(() => NoLetters);
            }
        }

        public override void GetClue()
        {
            // show to player that there are no more clues
            if(_lastSymbolIndexClues < 0 && Clue != Globals.noClues)
            {
                Clue = Globals.noClues;
            }
            else if(_lastSymbolIndexClues >= 0)
            {
                base.getClue();
                // hide symobl after right guess 
                CluesSymbols[_lastSymbolIndexClues].hide();
                _lastSymbolIndexClues -= 1;
            }
        }








    }

}
