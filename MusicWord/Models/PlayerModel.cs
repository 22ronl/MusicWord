using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    /// <summary>
    ///  Class <c>PlayerModel</c>  is a singelton. using to hold all relevant information of the player
    /// </summary>
    class PlayerModel
    {
        private static PlayerModel instance = null;
        private string _name;
        private string _category;
        private int _score;
        public string lastWord;
        private HashSet<string> played_words = new HashSet<string>();
        private PlayerModel() 
        {
            _score = 0;
        }

        /// <summary>method <c>Instance</c> return a new instance if didnt create before, and if already created return it's instance</summary>
        public static PlayerModel Instance
        {
            get
            {
                if (instance == null)
                {
                   
                    instance = new PlayerModel();
                }
                return instance;
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        /// <summary>method <c>isInSet</c> checks if the word to complete is already used in previose level</summary>
        public bool isInSet(string question)
        {
            if (played_words.Contains(question))
            {
                return false;
            }
            played_words.Add(question);
            return true;
        }
    
    }
}
