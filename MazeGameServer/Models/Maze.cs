using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
	public class Maze
	{
		// grid
		public int GridLayers { get; set; } = 4;
		public int GridWidth { get; set; } = 8;
		public int GridHeight { get; set; } = 8;
		public Cell[][][] MazeGrid { get; set; }

		// templating
		public Dictionary<string, int> StartLocation { get; set; }
		public Dictionary<string, int> EndLocation { get; set; }
		public string MazePath { get; set; }
		public string MazeTemplateCompressed { get; set; }
		public int MazeDifficulty { get; set; } = 0;
		public string BestPath { get; set; } = string.Empty;

		// status
		private bool MazeSolved { get; set; } = false;


		// utilities
		private Utils Utilities { get; }


		public Maze(int gridLayers, int gridWidth, int gridHeight, string mazeTemplateCompressed = null,
					Dictionary<string, int> startLocation = null, Dictionary<string, int> endLocation = null)
		{
			this.Utilities = new Utils();

			this.GridLayers = gridLayers;
			this.GridWidth = gridWidth;
			this.GridHeight = gridHeight;

			this.MazeGrid = this.GenerateGrid();

			if (mazeTemplateCompressed != null)
			{
				this.MazeTemplateCompressed = mazeTemplateCompressed;
				this.MazePath = this.FillMazeProcedural(mazeTemplateCompressed);
			}
			else
			{
				if (startLocation == null)
				{
					this.StartLocation = new Dictionary<string, int>();
					this.StartLocation.Add("Z", 0);
					this.StartLocation.Add("Y", 0);
					this.StartLocation.Add("X", 0);
				}
				else
				{
					this.StartLocation = startLocation;
				}

				if (endLocation == null)
				{
					Random rnd = new Random();

					this.EndLocation = new Dictionary<string, int>();
                    this.GridWidth = (this.GridWidth >= 2) ? this.GridWidth : 2;
                    this.GridHeight = (this.GridHeight >= 2) ? this.GridHeight : 2;

                    this.EndLocation.Add("Z", 0);
					this.EndLocation.Add("Y", rnd.Next(1, this.GridWidth - 1));
					this.EndLocation.Add("X", rnd.Next(1, this.GridHeight - 1));
				}
				else
				{
					this.EndLocation = endLocation;
				}
				this.MazePath = this.FillMazeRandom();

				this.MazeTemplateCompressed = this.Utilities.CompressTemplate(this);
			}
		}

		public void SetMazeSolvedToFalse()
		{
			this.MazeSolved = false;
		}

		public bool IsMazeSolved(Dictionary<string, int> currentLocation)
		{
			// If the maze has already been solved, don't change that fact
			if (this.MazeSolved)
			{
				return true;
			}
			else
			{
				return this.MazeSolved = (
						currentLocation["Z"] == this.EndLocation["Z"]
					&& 	currentLocation["Y"] == this.EndLocation["Y"]
					&& 	currentLocation["X"] == this.EndLocation["X"]
				);
			}
		}


		public void determineMazeDifficulty(int attempts = 3000)
		{
			int lowest = Int32.MaxValue;
			string path = string.Empty;

			for (int i = 0; i < attempts; i++)
			{
				MazeNavigator MyNavigator = new MazeNavigator(this);
				MyNavigator.Navigate();
				if (MyNavigator.attempts < lowest)
				{
					lowest = MyNavigator.attempts;
					path = MyNavigator.path;
				}
			}
			this.MazeDifficulty = lowest;
			this.BestPath = path;
		}

		private Cell[][][] GenerateGrid()
		{
			Cell[][][] tempGrid = new Cell[this.GridLayers][][];

			for (int i = 0; i < this.GridLayers; i++)
			{
				tempGrid[i] = new Cell[this.GridHeight][];
				for (int j = 0; j < this.GridHeight; j++)
				{
					tempGrid[i][j] = new Cell[this.GridWidth];
				}
			}
			return tempGrid;
		}

        private string FillMazeRandom()
        {
            int index = -1;
            string output = string.Empty;
            
            List<Cell> cellsList = new List<Cell>();
            cellsList.Add(new Cell(this.StartLocation["Z"], this.StartLocation["Y"], this.StartLocation["X"]));

            while (cellsList.Count > 0)
            {
                // index is the newest
                index = cellsList.Count - 1;

                Cell currentCell = cellsList[index];

                string[] directions = this.Utilities.GetRandomDirections();

                for (int i = 0; i < directions.Length; i++)
                {
                    Cell nextCell = this.DirectionModifier(cellsList[index], directions[i]);

                    if (this.IsEmptyCell(nextCell.Z, nextCell.Y, nextCell.X))
                    {
                        // we found a workable direction
                        // TODO: See if this works at all
                        this.CarvePathBetweenCells(ref currentCell, ref nextCell, directions[i]);
                        //this.MazeGrid[currentCell.Z][currentCell.Y][currentCell.X] = result.current;
                        //this.MazeGrid[nextCell.Z][nextCell.Y][nextCell.X] = result.next;

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

        private bool IsEmptyCell(int z, int y, int x)
        {
            if (
                    z >= 0 && z < this.GridLayers
                &&  y >= 0 && y < this.GridHeight
                &&  x >= 0 && x < this.GridWidth
                && this.MazeGrid[z][y][x] == null)
            {
                return true;
            }
            return false;
        }

        private string FillMazeProcedural(string mazeTemplateCompressed)
        {
            var tempMaze = this.Utilities.UncompressTemplate(mazeTemplateCompressed);

			//var p1 = tempMaze.MazePath.Split();
			//var p2 = p1.ToList();

			Stack<char> path = new Stack<char>(tempMaze.MazePath.ToArray().Reverse());
            this.StartLocation = tempMaze.StartLocation;
            this.EndLocation = tempMaze.EndLocation;
            this.BestPath = tempMaze.BestPath;
            this.MazeDifficulty = tempMaze.MazeDifficulty;
			this.GridWidth = tempMaze.GridWidth;
			this.GridHeight = tempMaze.GridHeight;
			this.GridLayers = tempMaze.GridLayers;
            this.MazeGrid = this.GenerateGrid();

            List<Cell> cellsList = new List<Cell>();
            cellsList.Add(new Cell(this.StartLocation["Z"], this.StartLocation["Y"], this.StartLocation["X"]));
            int index = -1;

            string mazePath = String.Empty;

            while (cellsList.Count > 0)
            {
                // index is the newest
				index = cellsList.Count - 1;

                Cell currentCell = cellsList[index];

				if(path.Count == 1)
				{
					break;
				}

				char n = path.Pop();
				string next = n.ToString();
                
                if (next == "" || next == null)
                {
                    break;
                }
                else if (next == Utils.Back)
                {
                    cellsList.Remove(cellsList[index]);
                }
                else 
                {
                    Cell nextCell = this.DirectionModifier(cellsList[index], next);
                    if (this.IsEmptyCell(nextCell.Z, nextCell.Y, nextCell.X))
                    {
                        this.CarvePathBetweenCells(ref currentCell, ref nextCell, next);

                        this.MazeGrid[currentCell.Z][currentCell.Y][currentCell.X] = currentCell;
                        this.MazeGrid[nextCell.Z][nextCell.Y][nextCell.X] = nextCell;
                        
                        cellsList.Add(nextCell);
                        index = -1;
                    }
                }
                if (index != -1 && cellsList.Count > 1)
                {
                    cellsList.Remove(cellsList[index -1]);
                }
                mazePath += next;
            }
            return mazePath;
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

		private Cell DirectionModifier(Cell cell, string direction)
		{
			switch (direction)
			{
				case Utils.North:
					return new Cell(cell.Z, cell.Y - 1, cell.X);
				case Utils.East:
					return new Cell(cell.Z, cell.Y, cell.X + 1);
				case Utils.South:
					return new Cell(cell.Z, cell.Y + 1, cell.X);
				case Utils.West:
					return new Cell(cell.Z, cell.Y, cell.X - 1);
				case Utils.Up:
					// if we're at the top layer, loop around
					if (cell.Z == this.GridLayers - 1)
						return new Cell(0, cell.Y, cell.X);
					else
						return new Cell(cell.Z + 1, cell.Y, cell.X);
				case Utils.Down:
					// if we're at the bottom layer, loop around
					if (cell.Z == 0)
						return new Cell(this.GridLayers - 1, cell.Y, cell.X);
					else
						return new Cell(cell.Z - 1, cell.Y, cell.X);
			}
			return new Cell(cell.Z, cell.Y, cell.Z);
		}
	}
}