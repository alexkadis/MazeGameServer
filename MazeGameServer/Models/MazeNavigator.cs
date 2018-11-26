using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class MazeNavigator
    {
        public int Moves { get; set; }
        public string Path { get; set; }
        private Maze MyMaze { get; set; }
        private Utils Utilities { get; }
        private Character Character { get; }

        public MazeNavigator(Maze maze)
        {
            this.Moves = 0;
            this.MyMaze = maze;
            this.Utilities = new Utils();
            Random rnd = new Random();
            this.Character = new Character("navigator", MyMaze);
        }

        public void Navigate()
        {
            while (!this.MyMaze.IsMazeSolved(this.Character.CurrentLocation))
            {
                var directions = this.Utilities.GetRandomDirections();
                for (var i = 0; i < directions.Length; i++)
                {
                    if (this.Character.CanMoveDirection(directions[i]))
                    {
                        this.Character.move(directions[i]);
                        this.Path += directions[i];
                        this.Moves++;
                        break;
                    }
                }
            }
            this.Character.ResetCharacter();
        }

        public bool IsNavigatablePath(string possiblePath)
        {
            bool isValidPath = false;
            Stack<char> path = new Stack<char>(possiblePath.ToArray().Reverse());

            for (int i = 0; i < path.Count; i++)
            {
                string next = path.Pop().ToString();
                if (this.Character.CanMoveDirection(next))
                {
                    this.Character.move(next);
                    this.Moves++;
                    break;
                }
            }
            return this.MyMaze.IsMazeSolved(this.Character.CurrentLocation);
        }
    }
}