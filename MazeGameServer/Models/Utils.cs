using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
	public class Utils
	{
		public const string North = "N";
		public const string East = "E";
		public const string South = "S";
		public const string West = "W";
		public const string Up = "U";
		public const string Down = "D";
		public readonly string[] Directions = new string[6]
			{
				Utils.North,
				Utils.South,
				Utils.West,
				Utils.East,
				Utils.Up,
				Utils.Down
			};
		public const string Back = "B";

		public Utils() {

		}

        public string CompressTemplate(Maze myMaze)
		{
            Dictionary<string, object> template = new Dictionary<string, object>();
            template.Add("MazePath", myMaze.MazePath);
			template.Add("StartLocation", this.SerializeLocation(myMaze.StartLocation));
			template.Add("EndLocation", this.SerializeLocation(myMaze.EndLocation)); //, Formatting.Indented));
            template.Add("BestPath", myMaze.BestPath);
            template.Add("MazeDifficulty", myMaze.MazeDifficulty);
			template.Add("GridWidth", myMaze.GridWidth);
			template.Add("GridHeight", myMaze.GridHeight);
			template.Add("GridLayers", myMaze.GridLayers);
			var jsonobj = JsonConvert.SerializeObject(template);
			var compressed = LZString.CompressToEncodedURIComponent(jsonobj);
			return compressed;
		}

        public Maze UncompressTemplate(string mazeTemplateCompressed)
        {
            var editedMaze = new Maze(0,0,0);
            try
            {
                string str = LZString.DecompressFromEncodedURIComponent(mazeTemplateCompressed);
				var templateAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

				editedMaze.MazePath = templateAsDictionary["MazePath"];
				editedMaze.StartLocation = this.DeserializeLocation(templateAsDictionary["StartLocation"]);
				editedMaze.EndLocation = this.DeserializeLocation(templateAsDictionary["EndLocation"]);
				editedMaze.BestPath = templateAsDictionary["BestPath"];
                editedMaze.MazeDifficulty = Convert.ToInt32(templateAsDictionary["MazeDifficulty"]);
				editedMaze.GridWidth = Convert.ToInt32(templateAsDictionary["GridWidth"]);
				editedMaze.GridHeight = Convert.ToInt32(templateAsDictionary["GridHeight"]);
				editedMaze.GridLayers = Convert.ToInt32(templateAsDictionary["GridLayers"]);
			}
            catch
            {
                // TODO: Error handling
                Console.WriteLine("Invalid Maze Template");
                throw;
            }
            return editedMaze;
        }


        // could create a Custom JsonConverter but this works, and I'm content with doing it the less efficient way
        // https://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm
        public Location DeserializeLocation(string str)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(str);
            var location = new Location(-1, -1, -1);
            if (dict.ContainsKey("Z") && dict.ContainsKey("Y") && dict.ContainsKey("X"))
            {
                location = new Location(dict["Z"], dict["Y"], dict["X"]);
            }

            return location;
        }

        public string SerializeLocation(Location location)
        {
            var dict = new Dictionary<string, int>();
            dict.Add("Z", location.Z);
            dict.Add("Y", location.Y);
            dict.Add("X", location.X);

            var str = JsonConvert.SerializeObject(dict);

            return str;
        }

        //public string[] GetRandomDirections()
        //{
        //    Random rnd = new Random();
        //    string[] temp = new string[this.Directions.Length];

        //    int j;
        //    int i;
        //    string x;
        //    for (i = this.Directions.Length - 1; i > 0; i--)
        //    {
        //        j = rnd.Next(0, this.Directions.Length - 1);
        //        x = temp[i];
        //        temp[i] = temp[j];
        //        temp[j] = x;
        //    }
        //    return temp;
        //}

        public string[] GetRandomDirections()
        {
            string[] temp = new string[this.Directions.Length];
            Array.Copy(this.Directions, temp, this.Directions.Length);
            Random rnd = new Random();
            int n = temp.Length;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = temp[k];
                temp[k] = temp[n];
                temp[n] = value;
            }
            return temp;
        }

        //public string[] GetRandomDirections()
        //{
        //    Random rnd = new Random();
        //    string[] temp = new string[this.Directions.Length];

        //    int j;
        //    int i;
        //    string x;
        //    for (i = this.Directions.Length - 1; i > 0; i--)
        //    {
        //        j = rnd.Next(0, this.Directions.Length - 1);
        //        x = temp[i];
        //        temp[i] = temp[j];
        //        temp[j] = x;
        //    }
        //    return temp;
        //}

    }
}



