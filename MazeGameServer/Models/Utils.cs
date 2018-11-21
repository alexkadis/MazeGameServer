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
			template.Add("StartLocation", JsonConvert.SerializeObject(myMaze.StartLocation));
			template.Add("EndLocation", JsonConvert.SerializeObject(myMaze.EndLocation)); //, Formatting.Indented));
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
				editedMaze.StartLocation = this.StringToLocation(templateAsDictionary["StartLocation"]));
				editedMaze.EndLocation = this.StringToLocation(templateAsDictionary["EndLocation"]);
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

        public Location StringToLocation(string str)
        {
            var location = new Location();
            var dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(str);
            location = this.ConvertDictionaryToLocation(dict);

            return location;
        }

        private Location ConvertDictionaryToLocation(Dictionary<string, int> dict)
        {
            var location = new Location();
            if (dict.ContainsKey("Z"))
            {
                location.Z = dict["Z"];
            }
            if (dict.ContainsKey("Y"))
            {
                location.Y = dict["Y"];
            }
            if (dict.ContainsKey("X"))
            {
                location.X = dict["X"];
            }
            return location;
        }

        // https://stackoverflow.com/questions/11576886/how-to-convert-object-to-dictionarytkey-tvalue-in-c/52961405#52961405
        //private static Dictionary<TKey, TValue> ObjectToDictionary<TKey, TValue>(object source)
        //{
        //    Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

        //    TKey[] keys = { };
        //    TValue[] values = { };

        //    bool outLoopingKeys = false, outLoopingValues = false;

        //    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
        //    {
        //        object value = property.GetValue(source);
        //        if (value is Dictionary<TKey, TValue>.KeyCollection)
        //        {
        //            keys = ((Dictionary<TKey, TValue>.KeyCollection)value).ToArray();
        //            outLoopingKeys = true;
        //        }
        //        if (value is Dictionary<TKey, TValue>.ValueCollection)
        //        {
        //            values = ((Dictionary<TKey, TValue>.ValueCollection)value).ToArray();
        //            outLoopingValues = true;
        //        }
        //        if (outLoopingKeys & outLoopingValues)
        //        {
        //            break;
        //        }
        //    }

        //    for (int i = 0; i < keys.Length; i++)
        //    {
        //        result.Add(keys[i], values[i]);
        //    }

        //    return result;
        //}

        public string[] GetRandomDirections()
        {
            Random rnd = new Random();
            string[] temp = new string[this.Directions.Length];

            int j;
            int i;
            string x;
            for (i = this.Directions.Length - 1; i > 0; i--)
            {
                j = rnd.Next(0, this.Directions.Length - 1);
                x = temp[i];
                temp[i] = temp[j];
                temp[j] = x;
            }
            return temp;
        }

    }
}



