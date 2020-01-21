using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    /// <summary>
    ///  Class <c>FinalPageViewModel</c> responsible on executing two bottums in FinalPageView.
    /// </summary>
    class FinalPageViewModel : AppScreen
    {
        public void PlayAgain()
        {
            ShellViewModel.Instance.playAgain();
            // close and open new connection
            SQLServerModel.Instance.closeConnection();
            SQLServerModel.Instance.connect();
            NextScreen();
        }
        public void EndGame()
        {
            SQLServerModel.Instance.closeConnection();
            System.Windows.Application.Current.Shutdown();

        }
    }
}
