using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class Game
    {
		public int GameId { get; set; }

		public DateTime GameDate { get; set; }

        public MazeTemplate MazeTemplate { get; set; }

        public string PathTaken { get; set; }

		public int Score { get { return PathTaken.Length; } }

        public int UserId { get; set; }

        public string Username { get; private set; }
        
        private void SetUsername(string username)
        {
            // TODO: just for the oldschool love, usernames can only be 3 characters long

        }

        public Game()
        {

        }
    }
}