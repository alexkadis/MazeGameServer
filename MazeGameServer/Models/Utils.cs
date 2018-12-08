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
		public readonly string[] Directions = 
			{
				Utils.North,
				Utils.South,
				Utils.West,
				Utils.East,
				Utils.Up,
				Utils.Down
			};

		public const string Back = "B";

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



