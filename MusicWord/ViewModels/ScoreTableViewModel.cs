using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
using Caliburn.Micro;
using System.Windows;

namespace MusicWord.ViewModels
{
    /// <summary>
    /// <c>ScoreTableViewModel</c>
    /// the score table viewModel
    /// </summary>
    class ScoreTableViewModel : AppScreen
    {
        /// <summary>
        /// a collection in which every item can be binded
        /// </summary>
        public BindableCollection<TableEntry> ScoreTable { get; set; }

        // init score table collection from the score table list in the SQLServerModel
        public ScoreTableViewModel()
        {
            ScoreTable = new BindableCollection<TableEntry>(SQLServerModel.getScoreTable());
        }
        /// <summary>
        /// function that makes the game move to the next window
        /// </summary>
        public void Next()
        {
            NextScreen();
        }
    }
}
