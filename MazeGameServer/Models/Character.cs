using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class Character
    {
        public string Name;
	    public Location CurrentLocation;
	    public Location PreviousLocation;
	    public Maze MyMaze;
	    //private Utils Utilities = new Utils();

        public Character (string name, Maze myMaze)
        {
            this.Name = name;
            this.MyMaze = myMaze;
            this.CurrentLocation = this.MyMaze.Template.StartLocation.Clone();
            this.Move(String.Empty);
        }

        public Character Clone()
        {
            return new Character
                (
                String.Copy(this.Name),
                this.MyMaze
                );
        }

        /**
         * If the direction parameter is valid, change the current location to that cell
         * @param direction Direction to move the character
         */
        public bool Move(string direction)
        {
            // Make a clean copy (not a reference)
            this.PreviousLocation = this.CurrentLocation.Clone();

            if (this.CanMoveDirection(direction))
            {
                switch (direction)
                {
                    case Utils.North:
                        this.SetRelativeLocation(0, -1, 0);
                        break;
                    case Utils.East:
                        this.SetRelativeLocation(0, 0, 1);
                        break;
                    case Utils.South:
                        this.SetRelativeLocation(0, 1, 0);
                        break;
                    case Utils.West:
                        this.SetRelativeLocation(0, 0, -1);
                        break;
                    case Utils.Up:
                        if (this.CurrentLocation.Z == this.MyMaze.Template.GridLayers - 1)
                        {
                            this.SetExactLocation(0, null, null);
                        }
                        else
                        {
                            this.SetRelativeLocation(1, 0, 0);
                        }
                        break;
                    case Utils.Down:
                        if (this.CurrentLocation.Z == 0)
                        {
                            this.SetExactLocation(this.MyMaze.Template.GridLayers - 1, null, null);
                        }
                        else
                        {
                            this.SetRelativeLocation(-1, 0, 0);
                        }
                        break;
                }
                return true;
            }
            return false;
        }

        public void ResetCharacter()
        {
            this.MyMaze.SetMazeSolvedToFalse();
            this.CurrentLocation = this.MyMaze.Template.StartLocation.Clone();
        }

        /**
         * Moves the character to an *exact* location within the grid
         * @param z Z-Axis
         * @param y Y-Axis
         * @param x X-Axis
         */
        public void SetExactLocation(int? z, int? y, int? x)
        {
            if (z != null)
            {
                this.CurrentLocation.Z = (int)z;
            }
            if (y != null)
            {
                this.CurrentLocation.Y = (int)y;
            }
            if (x != null)
            {
                this.CurrentLocation.X = (int)x;
            }
        }

        /**
         * Moves the character to a location relative to the current location within the grid
         * @param z Z-Axis
         * @param y Y-Axis
         * @param x X-Axis
         */
        public void SetRelativeLocation(int z, int y, int x)
        {
            this.CurrentLocation.Z += z;
            this.CurrentLocation.Y += y;
            this.CurrentLocation.X += x;
        }

        /**
         * Checks to see if a character can move a direction
         * @param direction The direction the character wants to move
         */
        public bool CanMoveDirection(string direction)
        {
            if (direction == Utils.Up || direction == Utils.Down)
            {
                return true;
            }

            if (this.CurrentLocation.IsValid(this.MyMaze.Template.GridLayers, this.MyMaze.Template.GridHeight, this.MyMaze.Template.GridWidth))
            {
                //try
                //{
                    var location = this.MyMaze.MazeGrid[this.CurrentLocation.Z][this.CurrentLocation.Y][this.CurrentLocation.X];
                    switch (direction)
                    {
                        case Utils.North:
                            return location.North;
                        case Utils.East:
                            return location.East;
                        case Utils.South:
                            return location.South;
                        case Utils.West:
                            return location.West;
                    }

                //}
                //catch (Exception ex)
                //{
                    //throw new Exception($"{this.CurrentLocation.ToString()} \n{ex}\n\n\n");
                //}
            }

            return false;
        }
    }
}
