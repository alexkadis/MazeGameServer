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

  //      public string CompressTemplate(Maze myMaze, bool encode = true)
		//{
  //          Dictionary<string, object> template = new Dictionary<string, object>();
  //          template.Add("MazePath", myMaze.MazePath);
  //          if (encode)
  //          {
  //              template.Add("StartLocation", this.SerializeLocation(myMaze.StartLocation));
  //              template.Add("EndLocation", this.SerializeLocation(myMaze.EndLocation));
  //          }
  //          else
  //          {
  //              template.Add("StartLocation", myMaze.StartLocation);
  //              template.Add("EndLocation", myMaze.EndLocation);
  //          }
  //          template.Add("BestPath", myMaze.BestPath);
  //          template.Add("MazeDifficulty", myMaze.MazeDifficulty);
		//	template.Add("GridWidth", myMaze.GridWidth);
		//	template.Add("GridHeight", myMaze.GridHeight);
		//	template.Add("GridLayers", myMaze.GridLayers);
		//	var compressed = JsonConvert.SerializeObject(template);
  //          if (encode)
  //          {
  //              compressed = LZString.CompressToEncodedURIComponent(compressed);
  //          }
  //          return compressed;
		//}

   //     public Maze UncompressTemplate(string mazeTemplateCompressed)
   //     {
   //         var editedMaze = new Maze(0,0,0);
   //         try
   //         {
   //             string str = LZString.DecompressFromEncodedURIComponent(mazeTemplateCompressed);
			//	var templateAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

			//	editedMaze.MazePath = templateAsDictionary["MazePath"];
			//	editedMaze.StartLocation = this.DeserializeLocation(templateAsDictionary["StartLocation"]);
			//	editedMaze.EndLocation = this.DeserializeLocation(templateAsDictionary["EndLocation"]);
			//	editedMaze.BestPath = templateAsDictionary["BestPath"];
   //             editedMaze.MazeDifficulty = Convert.ToInt32(templateAsDictionary["MazeDifficulty"]);
			//	editedMaze.GridWidth = Convert.ToInt32(templateAsDictionary["GridWidth"]);
			//	editedMaze.GridHeight = Convert.ToInt32(templateAsDictionary["GridHeight"]);
			//	editedMaze.GridLayers = Convert.ToInt32(templateAsDictionary["GridLayers"]);
			//}
   //         catch
   //         {
   //             // TODO: Error handling
   //             Console.WriteLine("Invalid Maze Template");
   //             throw;
   //         }
   //         return editedMaze;
   //     }


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

    }
}



