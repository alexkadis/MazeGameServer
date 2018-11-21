using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    internal class MazeNavigator
    {
        internal int attempts;
        internal string path;
        private Maze maze;

        public MazeNavigator(Maze maze)
        {
            this.maze = maze;
            this.Utilities = new Utils();
            this.Character = new Character("navigator" + this.Utilities.getRandomIntInclusive(0, 1000), myMaze);
        }

        internal void Navigate()
        {
            let moved = false;
            while (!this.MyMaze.IsMazeSolved(this.Character.CurrentLocation))
            {
                const directions = this.Utilities.getRandomDirections();
                for (let i = 0; i < directions.length; i++)
                {
                    if (this.Character.CanMoveDirection(directions[i]))
                    {
                        this.Character.move(directions[i]);
                        this.path += directions[i];
                        this.attempts++;
                        moved = true;
                        break;
                    }
                }
                if (!moved)
                {
                    this.Character.CurrentLocation = this.Character.PreviousLocation;
                }
                moved = false;
            }
            this.Character.ResetCharacter();
        }
    }
    }
}