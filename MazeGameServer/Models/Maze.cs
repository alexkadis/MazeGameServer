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
        private Utils Utilities { get; }

        public Maze(MazeTemplate mazeTemplate)
        {
            Template = mazeTemplate;
            this.MazeGrid = this.GenerateGrid(Template.GridLayers, Template.GridHeight, Template.GridWidth);
            this.FillMazeProcedural();
        }

        public Maze(string mazeTemplateCompressed)
        {
            this.Utilities = new Utils();

            Template = new MazeTemplate(mazeTemplateCompressed);
            this.MazeGrid = this.GenerateGrid(Template.GridLayers, Template.GridHeight, Template.GridWidth);
            this.FillMazeProcedural();
        }

        public Maze(int gridLayers = 4, int gridWidth = 8, int gridHeight = 8,
                    Location startLocation = null, Location endLocation = null)
        {
            this.Utilities = new Utils();

			//gridWidth = (gridWidth >= 2) ? gridWidth : 2;
			//gridHeight = (gridHeight >= 2) ? gridHeight : 2;

            this.MazeGrid = this.GenerateGrid(gridLayers, gridHeight, gridWidth);

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
                        rnd.Next(1, gridWidth - 1),
                        rnd.Next(1, gridHeight - 1)
                    );
            }

            Template = new MazeTemplate(startLocation, endLocation, gridLayers, gridHeight, gridWidth, this.FillMazeRandom());
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
            else
            {
                return this.MazeSolved = currentLocation.Equals(Template.EndLocation);
            }
        }
        
        public void DetermineMazeDifficulty(int totalAttempts = 10000, int maximumMovesPerAttempt = 100, int numberOfRounds = 100)
        {
            int lowest = Int32.MaxValue;
            string path = string.Empty;

            MazeNavigator MyNavigator = new MazeNavigator(this);

            // deal with divide by zero
            if (numberOfRounds < 1)
            {
                numberOfRounds = 1;
            }

            var attemptsThisRound = (totalAttempts / numberOfRounds);

            for (int round = 0; round < numberOfRounds; round++)
            {
                for (int attempt = 0; attempt < attemptsThisRound; attempt++)
                {
                    MyNavigator.Navigate(maximumMovesPerAttempt);
                    // We want to ignore any number of moves above `maximumMovesPerAttempt`
                    if (MyNavigator.Moves == -1)
                    {
                        continue;
                    }
                    if (MyNavigator.Moves < lowest)
                    {
                        lowest = MyNavigator.Moves;
                        path = MyNavigator.Path;
                    }
                }
                
                if (lowest != Int32.MaxValue)
                {
                    // we got some result under `maximumMovesPerAttempt`
                    Template.MazeDifficulty = lowest;
                    Template.BestPath = path;
                    return;
                }
            }
            Template.MazeDifficulty = -1;
            Template.BestPath = "Unable to find best path.";
        }

        //// TODO: Figure out how to make this work, and figure out how to make it memory efficent
        //public void DetermineMazeDifficultyParallel(int attempts = 3000)
        //{
        //    var possiblePaths = new ConcurrentBag<string>();

        //    MazeNavigator MyNavigator = new MazeNavigator(this);
        //    Parallel.For(0, attempts, i =>
        //    {
        //        MyNavigator.Navigate();
        //        try
        //        {
        //            possiblePaths.Add(MyNavigator.Path);
        //        }
        //        catch
        //        {

        //        }
        //    });
        //    var paths = possiblePaths.ToArray();
        //    var difficulty = paths.Min(p => p.Length);
        //    var bestPath = paths.FirstOrDefault(p => p.Length == difficulty);

        //    this.MazeDifficulty = difficulty;
        //    this.BestPath = bestPath;
        //}

        private Cell[][][] GenerateGrid(int layers, int height, int width)
        {
			this.GridLayers = layers;
			this.GridHeight = height;
			this.GridWidth = width;

			Cell[][][] tempGrid = new Cell[layers][][];

            for (int i = 0; i < layers; i++)
            {
                tempGrid[i] = new Cell[height][];
                for (int j = 0; j < height; j++)
                {
                    tempGrid[i][j] = new Cell[width];
                }
            }
            return tempGrid;
        }

        private string FillMazeRandom()
        {
            int index = -1;
            string output = string.Empty;

            List<Cell> cellsList = new List<Cell>()
            {
                //new Cell(this.StartLocation.Z, this.StartLocation.Y, this.StartLocation.X)
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
                    Cell nextCell = this.DirectionModifier(cellsList[index], directions[i]);

                    if (this.IsEmptyCell(nextCell))
                    {
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

        private bool IsEmptyCell(Cell cell)
        {
			var valid = IsValidCell(cell);
			try 
			{
				var place = (this.MazeGrid[cell.Location.Z][cell.Location.Y][cell.Location.X] == null);
				return valid & place;
			}
			catch
			{

			}
			return false;
		}

        public bool IsValidCell(Cell cell)
        {
			return cell.Location.IsValid(this.GridLayers, this.GridHeight, this.GridWidth);
        }

        private void FillMazeProcedural()
        {
            Stack<char> path = new Stack<char>(Template.MazePath.ToArray().Reverse());

            List<Cell> cellsList = new List<Cell>
            {
                new Cell(Template.StartLocation.Clone())
            };

            int index = -1;

            //string mazePath = String.Empty;

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
                    Cell nextCell = this.DirectionModifier(cellsList[index], next);
                    if (this.IsValidCell(nextCell))
                    {
                        this.CarvePathBetweenCells(ref currentCell, ref nextCell, next);

                        this.AddCellToGrid(currentCell);
                        this.AddCellToGrid(nextCell);

                        cellsList.Add(nextCell);
                    }
                    //else
                    //{
                    //    throw new Exception($"Invalid Cell: [{currentCell.Z}][{currentCell.Y}][{currentCell.X}] > {next} > [{nextCell.Z}][{nextCell.Y}][{nextCell.X}]\n{Template.MazePath}\n\n\n{mazePath}");
                    //}
                }
                //mazePath += next;
            }
            //return mazePath;
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

        private Cell DirectionModifier(Cell cell, string direction)
        {
            switch (direction)
            {
                case Utils.North:
                    return new Cell(new Location(cell.Location.Z, cell.Location.Y - 1, cell.Location.X));
                case Utils.South:
                    return new Cell(new Location(cell.Location.Z, cell.Location.Y + 1, cell.Location.X));
                case Utils.West:
                    return new Cell(new Location(cell.Location.Z, cell.Location.Y, cell.Location.X - 1));
                case Utils.East:
                    return new Cell(new Location(cell.Location.Z, cell.Location.Y, cell.Location.X + 1));
                case Utils.Up:
                    // if we're at the top layer, loop around
                    if (cell.Location.Z == this.GridLayers - 1)
                        return new Cell(new Location(0, cell.Location.Y, cell.Location.X));
                    else
                        return new Cell(new Location(cell.Location.Z, cell.Location.Y, cell.Location.X));
                case Utils.Down:
                    // if we're at the bottom layer, loop around
                    if (cell.Location.Z == 0)
                        return new Cell(new Location(this.GridLayers - 1, cell.Location.Y, cell.Location.X));
                    else
                        return new Cell(new Location(cell.Location.Z - 1, cell.Location.Y, cell.Location.X));
            }
            return cell.Clone();
        }
    }
}