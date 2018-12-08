using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
	public class Maze
	{
		public MazeTemplate Template { get; private set; }

		private Location StartLocation { get; set; }
		private int GridLayers { get; set; }
		private int GridHeight { get; set; }
		private int GridWidth { get; set; }

		public Cell[][][] MazeGrid { get; set; }

		// status
		private bool MazeSolved { get; set; } = false;

		// utilities
		private Utils Utilities { get; } = new Utils();

		public Maze(MazeTemplate mazeTemplate)
		{
			Template = mazeTemplate;
			this.SetGridDimensions(Template.GridLayers, Template.GridHeight, Template.GridWidth);
			this.MazeGrid = this.GenerateGrid();
			this.FillMazeProcedural();
		}

		public Maze(string mazeTemplateCompressed)
		{
			Template = new MazeTemplate(mazeTemplateCompressed);
			this.SetGridDimensions(Template.GridLayers, Template.GridHeight, Template.GridWidth);
			this.MazeGrid = this.GenerateGrid();
			this.FillMazeProcedural();
		}

		public Maze(int gridLayers = 4, int gridWidth = 8, int gridHeight = 8,
					Location startLocation = null, Location endLocation = null)
		{
			this.SetGridDimensions(gridLayers, gridHeight, gridWidth);
			this.MazeGrid = this.GenerateGrid();

			if (startLocation == null)
			{
				startLocation = new Location(0, 0, 0);
			}
			this.StartLocation = startLocation;

			if (endLocation == null)
			{
				Random rnd = new Random();
				endLocation = new Location
					(
						0,
						rnd.Next(1, GridWidth - 1),
						rnd.Next(1, GridHeight - 1)
					);
			}

			Template = new MazeTemplate(startLocation, endLocation, GridLayers, GridHeight, GridWidth, this.FillMazeRandom());
		}

		public void SetMazeSolvedToFalse()
		{
			this.MazeSolved = false;
		}

		public bool IsMazeSolved(Location currentLocation)
		{
			// If the maze has already been solved, don't change that fact
			if (this.MazeSolved)
			{
				return true;
			}

			return this.MazeSolved = currentLocation.Equals(Template.EndLocation);
		}

		public void SetGridDimensions(int layers, int height, int width)
		{
			// set limits for what size a maze can be:
			this.GridLayers = Utilities.ForceNumberToBeWithinRange(layers, 2, 10);
			this.GridHeight = Utilities.ForceNumberToBeWithinRange(height, 2, 10);
			this.GridWidth = Utilities.ForceNumberToBeWithinRange(width, 2, 10);
		}

		private Cell[][][] GenerateGrid()
		{
			Cell[][][] tempGrid = new Cell[GridLayers][][];

			for (int i = 0; i < GridLayers; i++)
			{
				tempGrid[i] = new Cell[GridHeight][];
				for (int j = 0; j < GridHeight; j++)
				{
					tempGrid[i][j] = new Cell[GridWidth];
				}
			}
			return tempGrid;
		}

		private string FillMazeRandom()
		{
			int index = -1;
			string output = string.Empty;

			List<Cell> cellsList = new List<Cell>
			{
				new Cell(this.StartLocation)
			};

			while (cellsList.Count > 0)
			{
				// index is the newest
				index = cellsList.Count - 1;

				Cell currentCell = cellsList[index];

				string[] directions = this.Utilities.GetRandomDirections();

				for (int i = 0; i < directions.Length; i++)
				{
					Location nextLocation = this.DirectionModifier(cellsList[index].Location, directions[i]);
					if (this.IsValidLocation(nextLocation) && this.IsEmptyLocation(nextLocation))
					{
						Cell nextCell = new Cell(nextLocation);
						// we found a workable direction
						this.CarvePathBetweenCells(ref currentCell, ref nextCell, directions[i]);

						this.AddCellToGrid(currentCell);
						this.AddCellToGrid(nextCell);

						cellsList.Add(nextCell);
						output += directions[i];
						index = -1;
						break;
					}
				}
				if (index != -1)
				{
					cellsList.Remove(cellsList[index]);
					output += Utils.Back;
				}
			}
			return output;
		}

		public bool IsValidLocation(Location location)
		{
			return location.IsValid(this.GridLayers, this.GridHeight, this.GridWidth);
		}

		private bool IsEmptyLocation(Location location)
		{
			return (this.MazeGrid[location.Z][location.Y][location.X] == null);
		}

		private void FillMazeProcedural()
		{
			Stack<char> path = new Stack<char>(Template.MazePath.ToArray().Reverse());

			List<Cell> cellsList = new List<Cell>
			{
				new Cell(Template.StartLocation.Clone())
			};

			int index = -1;

			while (cellsList.Count > 0)
			{
				// index is the newest
				index = cellsList.Count - 1;

				Cell currentCell = cellsList[index];

				string next = path.Pop().ToString();

				if (next == Utils.Back)
				{
					cellsList.Remove(cellsList[index]);
				}
				else
				{
					Location nextLocation = this.DirectionModifier(cellsList[index].Location, next);
					if (this.IsValidLocation(nextLocation))
					{
						Cell nextCell = new Cell(nextLocation);
						this.CarvePathBetweenCells(ref currentCell, ref nextCell, next);

						this.AddCellToGrid(currentCell);
						this.AddCellToGrid(nextCell);

						cellsList.Add(nextCell);
					}
				}
			}
		}

		private void AddCellToGrid(Cell cell)
		{
			this.MazeGrid[cell.Location.Z][cell.Location.Y][cell.Location.X] = cell;
		}

		private void CarvePathBetweenCells(ref Cell currentCell, ref Cell nextCell, string direction)
		{
			if (this.Utilities != null)
			{
				switch (direction)
				{
					case Utils.North:
						currentCell.North = true;
						nextCell.South = true;
						break;
					case Utils.East:
						currentCell.East = true;
						nextCell.West = true;
						break;
					case Utils.South:
						currentCell.South = true;
						nextCell.North = true;
						break;
					case Utils.West:
						currentCell.West = true;
						nextCell.East = true;
						break;
					case Utils.Up:
						currentCell.Up = true;
						nextCell.Down = true;
						break;
					case Utils.Down:
						currentCell.Down = true;
						nextCell.Up = true;
						break;
				}
			}
		}

		private Location DirectionModifier(Location location, string direction)
		{
			switch (direction)
			{
				case Utils.North:
					return new Location(location.Z, location.Y - 1, location.X);
				case Utils.South:
					return new Location(location.Z, location.Y + 1, location.X);
				case Utils.West:
					return new Location(location.Z, location.Y, location.X - 1);
				case Utils.East:
					return new Location(location.Z, location.Y, location.X + 1);
				case Utils.Up:
					// if we're at the top layer, loop around
					if (location.Z == this.GridLayers - 1)
					{
						return new Location(0, location.Y, location.X);
					}
					return new Location(location.Z, location.Y, location.X);
				case Utils.Down:
					// if we're at the bottom layer, loop around
					if (location.Z == 0)
					{
						return new Location(this.GridLayers - 1, location.Y, location.X);
					}
					return new Location(location.Z - 1, location.Y, location.X);
			}
			return location.Clone();
		}
	}
}