using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairsGame
{
    public class User
    {
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public User()
        {
            Username = "";
            ProfilePicture = "";
            GamesPlayed = 0;
            GamesWon = 0;
        }
    }
}
