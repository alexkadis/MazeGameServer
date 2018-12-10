using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class Game
    {
        public DateTime GameDate { get; set; }

        public int MazeId { get; set; }

        public string PathTaken { get; set; }

        public int Score { get { return PathTaken.Length; } }

        public int UserId { get; set; }

        public string Username { get; set; }
        
        private void SetUsername(string username)
        {
            // just for the oldschool love, usernames can only be 3 characters long

        }

        public Game()
        {

        }
    }
}