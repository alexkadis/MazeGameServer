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
        private Character Character { get; set; }

        public MazeNavigator(Maze maze)
        {
            this.Moves = 0;
            this.MyMaze = maze;
            this.Utilities = new Utils();
            this.Character = new Character("navigator", MyMaze);
        }

        public void Navigate(int maxMoves = 500)
        {
            this.Reset();
            while (true)
            {
                if (this.Moves >= maxMoves)
                {
                    this.Moves = -1;
                    return;
                }
                var directions = this.Utilities.GetRandomDirections();
                for (var i = 0; i < directions.Length; i++)
                {
                    if (this.Character.CanMoveDirection(directions[i]))
                    {
                        this.Character.Move(directions[i]);
                        this.Path += directions[i];
                        this.Moves++;
                        break;
                    }
                }
                if (this.Character.CurrentLocation.Equals(this.MyMaze.Template.EndLocation))
                {
                    break;
                }
            }
        }

        private void Reset()
        {
            //this.Character = this.Character.Clone();
            this.Character.CurrentLocation = this.MyMaze.Template.StartLocation.Clone();
            this.Path = String.Empty;
            this.Moves = 0;
        }

        public bool IsNavigatablePath(string possiblePath)
        {
			this.Moves = 0;
            Stack<char> path = new Stack<char>(possiblePath.ToArray().Reverse());
            int i = 0;
            for (i = 0; i < possiblePath.Length; i++)
            {
                string next = path.Pop().ToString();
                if (this.Character.CanMoveDirection(next))
                {
                    this.Character.Move(next);
                    this.Moves++;
                }
                else
                {
                    //return false;
                    //throw new Exception($"Invalid direction: {next}");
                    throw new Exception("Invalid Direction in Path");
                }
            }
            return this.Character.CurrentLocation.Equals(this.MyMaze.Template.EndLocation);
        }
    }
}