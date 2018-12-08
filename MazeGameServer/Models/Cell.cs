using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
	public class Cell
	{
		public bool North { get; set; }
		public bool South { get; set; }
		public bool East { get; set; }
		public bool West { get; set; }
		public bool Up { get; set; }
		public bool Down { get; set; }
        public Location Location { get; set; }

        public Cell(Location location)
        {
			this.North = false;
			this.East = false;
			this.South = false;
			this.West = false;
			this.Up = false;
			this.Down = false;
            this.Location = location;
		}

        public Cell Clone()
        {
            var newCell = new Cell(this.Location.Clone())
            {
                North = this.North,
                East = this.East,
                South = this.South,
                West = this.West,
                Up = this.Up,
                Down = this.Down
            };

            return newCell;
        }
	}
}
