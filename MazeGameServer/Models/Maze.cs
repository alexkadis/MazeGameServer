﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public string MazePath { get; set; }
        public string MazeTemplateCompressed { get; set; }
        public int MazeDifficulty { get; set; } = 0;
        public string BestPath { get; set; } = string.Empty;

        // status
        private bool MazeSolved { get; set; } = false;


        // utilities
        private Utils Utilities { get; }


        public Maze(int gridLayers, int gridWidth, int gridHeight, string mazeTemplateCompressed = null,
                    Location startLocation = null, Location endLocation = null)
        {
            this.Utilities = new Utils();

            this.GridLayers = gridLayers;
            this.GridWidth = gridWidth;
            this.GridHeight = gridHeight;

            this.MazeGrid = this.GenerateGrid();

            if (mazeTemplateCompressed != null)
            {
                this.MazeTemplateCompressed = mazeTemplateCompressed;
                this.TemplateToSelf();
                this.MazePath = this.FillMazeProcedural();
            }
            else
            {
                if (startLocation == null)
                {
                    this.StartLocation = new Location(0, 0, 0);
                }
                else
                {
                    this.StartLocation = startLocation;
                }

                if (endLocation == null)
                {
                    Random rnd = new Random();

                    this.GridWidth = (this.GridWidth >= 2) ? this.GridWidth : 2;
                    this.GridHeight = (this.GridHeight >= 2) ? this.GridHeight : 2;

                    this.EndLocation = new Location
                        (
                            0,
                            rnd.Next(1, this.GridWidth - 1),
                            rnd.Next(1, this.GridHeight - 1)
                        );
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

        public bool IsMazeSolved(Location currentLocation)
        {
            // If the maze has already been solved, don't change that fact
            if (this.MazeSolved)
            {
                return true;
            }
            else
            {
                return this.MazeSolved = currentLocation.Equals(this.EndLocation);
            }
        }

        //public void DebugOutputMaze()
        //{
        //    for (int z = 0; z < this.GridLayers; z++)
        //    {
        //        for (int y = 0; y < this.GridWidth; y++)
        //        {
        //            string row1 = String.Empty;
        //            string row2 = String.Empty;
        //            string row3 = String.Empty;
        //            for (int x = 0; x < this.GridHeight; x++)
        //            {
        //                var borders = OutputCell(this.MazeGrid[z][y][x]);
        //                row1 += borders[0];
        //                row2 += borders[1];
        //                row3 += borders[2];
        //            }
        //            Debug.WriteLine(row1);
        //            Debug.WriteLine(row2);
        //            Debug.WriteLine(row3);
        //        }
        //    }
        //}

        //private string[] OutputCell(Cell cell)
        //{
        //    string row1 = String.Empty;
        //    string row2 = String.Empty;
        //    string row3 = String.Empty;
        //    if (!cell.West)
        //    {
        //        row1 += "+";
        //        row2 += "|";
        //        row3 += "+";
        //    }
        //    else
        //    {
        //        row1 += " ";
        //        row2 += " ";
        //        row3 += " ";
        //    }
        //    if (!cell.North && !cell.South)
        //    {
        //        row1 += "—";
        //        row2 += " ";
        //        row3 += "—";
        //    }
        //    else if (!cell.North)
        //    {
        //        row1 += "—";
        //        row2 += " ";
        //        row3 += " ";
        //    }
        //    else if (!cell.South)
        //    {
        //        row1 += " ";
        //        row2 += " ";
        //        row3 += "—";
        //    }
        //    if (!cell.East)
        //    {
        //        row1 += "+";
        //        row2 += "|";
        //        row3 += "+";
        //    }
        //    else
        //    {
        //        row1 += " ";
        //        row2 += " ";
        //        row3 += " ";
        //    }
        //    return new string[] { row1, row2, row3 };
        //}

        public void DetermineMazeDifficulty(int attempts = 3000)
        {
            int lowest = Int32.MaxValue;
            string path = string.Empty;

            for (int i = 0; i < attempts; i++)
            {
                MazeNavigator MyNavigator = new MazeNavigator(this);
                MyNavigator.Navigate();
                if (MyNavigator.Moves < lowest)
                {
                    lowest = MyNavigator.Moves;
                    path = MyNavigator.Path;
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
            cellsList.Add(new Cell(this.StartLocation.Z, this.StartLocation.Y, this.StartLocation.X));

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
            return (IsValidCell(cell) && this.MazeGrid[cell.Z][cell.Y][cell.X] == null);
        }

        private bool IsValidCell(Cell cell)
        {
            return (cell.Z >= 0 && cell.Z < this.GridLayers
                && cell.Y >= 0 && cell.Y < this.GridHeight
                && cell.X >= 0 && cell.X < this.GridWidth);
        }

        private void TemplateToSelf()
        {
            var tempMaze = this.Utilities.UncompressTemplate(this.MazeTemplateCompressed);
            this.MazePath = tempMaze.MazePath;
            this.StartLocation = tempMaze.StartLocation;
            this.EndLocation = tempMaze.EndLocation;
            this.BestPath = tempMaze.BestPath;
            this.MazeDifficulty = tempMaze.MazeDifficulty;
            this.GridWidth = tempMaze.GridWidth;
            this.GridHeight = tempMaze.GridHeight;
            this.GridLayers = tempMaze.GridLayers;
            this.MazeGrid = this.GenerateGrid();
        }

        private string FillMazeProcedural()
        {
            Stack<char> path = new Stack<char>(this.MazePath.ToArray().Reverse());

            List<Cell> cellsList = new List<Cell>();
            List<Cell> cellsListBackup = new List<Cell>();
            cellsList.Add(new Cell(this.StartLocation.Z, this.StartLocation.Y, this.StartLocation.X));
            int index = -1;

            string mazePath = String.Empty;

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
                        cellsListBackup.Add(nextCell);
                    }
                    else
                    {
                        //cellsList.Add(currentCell);
                        //cellsListBackup.Add(currentCell);
                        //index = -1;
                        var temp = "";
                        throw new Exception($"Invalid Cell: [{currentCell.Z}][{currentCell.Y}][{currentCell.X}] > {next} > [{nextCell.Z}][{nextCell.Y}][{nextCell.X}]\n{this.MazePath}\n\n\n{mazePath}");
                    }
                }
                mazePath += next;
            }
            return mazePath;
        }

        private void AddCellToGrid(Cell cell)
        {
            this.MazeGrid[cell.Z][cell.Y][cell.X] = cell;
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