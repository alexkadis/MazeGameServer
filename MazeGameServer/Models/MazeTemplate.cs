using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class MazeTemplate
    {
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public int GridLayers { get; set; }
        public int GridHeight { get; set; }
        public int GridWidth { get; set; }
        public string MazePath { get; set; }
        public string BestPath { get; set; }
        public int MazeDifficulty
        {
            get
            {
                if (BestPath.Length > 0)
                    return BestPath.Length;
                return -1;
            }
        }
		public int MazeId { get; set; }

        [JsonConstructor]
        public MazeTemplate(Location startLocation, Location endLocation, int gridLayers, int gridHeight, int gridWidth, string mazePath = null, string bestPath = null)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            GridLayers = gridLayers;
            GridHeight = gridHeight;
            GridWidth = gridWidth;
            MazePath = mazePath;
            BestPath = bestPath;
        }
        
        public MazeTemplate(MazeTemplate mazeTemplate)
        {
            StartLocation = mazeTemplate.StartLocation;
            EndLocation = mazeTemplate.EndLocation;
            GridLayers = mazeTemplate.GridLayers;
            GridHeight = mazeTemplate.GridHeight;
            GridWidth = mazeTemplate.GridWidth;
            MazePath = mazeTemplate.MazePath;
            BestPath = mazeTemplate.BestPath;
        }

        public MazeTemplate()
        {
        }

        public MazeTemplate(string mazeTemplateCompressed)
        {
            this.Uncompress(mazeTemplateCompressed);
        }

        public void Uncompress(string mazeTemplateCompressed)
        {
            try
            {
                //string str = LZString.DecompressFromEncodedURIComponent(mazeTemplateCompressed);
                var templateAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(mazeTemplateCompressed);

                this.MazePath = templateAsDictionary["MazePath"];
                this.StartLocation = new Location(templateAsDictionary["StartLocation"]);
                this.EndLocation = new Location(templateAsDictionary["EndLocation"]);
                this.BestPath = templateAsDictionary["BestPath"];
                this.GridWidth = Convert.ToInt32(templateAsDictionary["GridWidth"]);
                this.GridHeight = Convert.ToInt32(templateAsDictionary["GridHeight"]);
                this.GridLayers = Convert.ToInt32(templateAsDictionary["GridLayers"]);
            }
            catch
            {
                // TODO: Error handling
                Console.WriteLine("Invalid Maze Template");
                throw;
            }
        }

        public string Compress() //(bool encode = false)
        {
            Dictionary<string, object> template = new Dictionary<string, object>
            {
                { "MazePath", this.MazePath },
                { "StartLocation", this.StartLocation},
                { "EndLocation", this.EndLocation},
                { "BestPath", this.BestPath},
                { "GridWidth", this.GridWidth},
                { "GridHeight", this.GridHeight},
                { "GridLayers", this.GridLayers}
            };
            
            var compressed = JsonConvert.SerializeObject(template);
            //if (encode)
            //{
            //    compressed = LZString.CompressToEncodedURIComponent(compressed);
            //}
            return compressed;
        }

        public MazeTemplate Clone()
        {
            return new MazeTemplate(StartLocation.Clone(), EndLocation.Clone(), GridLayers, GridHeight, GridWidth, MazePath, BestPath);
        }

		public void SetBestPath()
		{
			var maze = new Maze(this);
			MazeNavigator mazeNavigator = new MazeNavigator(maze);

			// if it already has a valid best path, just leave it as-is
			if (!string.IsNullOrEmpty(BestPath))
			{
				if(mazeNavigator.IsNavigatablePath(BestPath) && BestPath.Length == MazeDifficulty)
				{
					return;
				}
			}
			DetermineMazeDifficulty(maze);
		}

		public void DetermineMazeDifficulty(Maze maze = null, int totalAttempts = 5000, int maximumMovesPerAttempt = 100, int numberOfRounds = 500)
		{
			int lowest = Int32.MaxValue;
			string path = string.Empty;

			if (maze == null)
			{
				maze = new Maze(this);
			}
			MazeNavigator mazeNavigator = new MazeNavigator(maze);

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
					mazeNavigator.Navigate(maximumMovesPerAttempt);
					// We want to ignore any number of moves above `maximumMovesPerAttempt`
					if (mazeNavigator.Moves == -1)
					{
						continue;
					}
					if (mazeNavigator.Moves < lowest)
					{
						lowest = mazeNavigator.Moves;
						path = mazeNavigator.Path;
					}
				}

				if (lowest != Int32.MaxValue)
				{
					// we got some result under `maximumMovesPerAttempt`
					BestPath = path;
					return;
				}
			}
			BestPath = null;
		}

		public bool IsValid()
		{
			var navigatable = true;
			if (!string.IsNullOrEmpty(BestPath))
			{
				MazeNavigator mazeNavigator = new MazeNavigator(new Maze(this));
                navigatable = mazeNavigator.IsNavigatablePath(BestPath);
			}
			return (GridLayers > 0)
				&& (GridHeight > 0)
				&& (GridWidth > 0)
				&& StartLocation.IsValid(GridLayers, GridWidth, GridHeight)
				&& EndLocation.IsValid(GridLayers, GridWidth, GridHeight)
                && MazePathIsValid()
                && navigatable;
		}

		private bool MazePathIsValid()
		{
			var dimensions = (double)GridLayers * GridHeight * GridWidth;
            var pathLength = ((double)MazePath.Length + 1) / 2;

			// https://stackoverflow.com/questions/1398753/comparing-double-values-in-c-sharp
			if (Math.Abs(dimensions - pathLength) > 0.001)
			{
				return false;
			}

			var utils = new Utils();
            var directions = new string[utils.Directions.Length + 1];
            utils.Directions.CopyTo(directions,0);
            directions[directions.Length - 1] = Utils.Back;

            foreach (char direction in MazePath)
			{
				if(!directions.Contains(direction.ToString()))
				{
					return false;
				}
			}

			return true;
		}

		public bool Equals(MazeTemplate mazeTemplate)
		{
			return GridLayers == mazeTemplate.GridLayers
				&& GridHeight == mazeTemplate.GridHeight
				&& GridWidth == mazeTemplate.GridWidth
			  	&& MazePath == mazeTemplate.MazePath
				&& MazeId == mazeTemplate.MazeId
				&& StartLocation.Equals(mazeTemplate.StartLocation)
				&& EndLocation.Equals(mazeTemplate.EndLocation);
		}

    }
}
