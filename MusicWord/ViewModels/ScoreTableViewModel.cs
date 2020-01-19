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
    
    class ScoreTableViewModel : AppScreen
    {
        public BindableCollection<TableEntry> ScoreTable { get; set; }

        public ScoreTableViewModel()
        {
            ScoreTable = new BindableCollection<TableEntry>(SQLServerModel.getScoreTable());
        }
        public void PlayAgain()
        {
            ShellViewModel.Instance.playAgain();
            NextScreen();
        }
    }
}
