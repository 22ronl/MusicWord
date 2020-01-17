using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
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
