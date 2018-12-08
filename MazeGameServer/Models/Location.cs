using Newtonsoft.Json;
using System.Collections.Generic;

namespace MazeGameServer.Models
{
    public class Location
    {
        public int Z { get; set; }
        public int Y { get; set; }
        public int X { get; set; }

        public Location(string locationString)
        {
            this.Deserialize(locationString);
        }
        public Location(int z, int y, int x)
        {
            this.Z = z;
            this.Y = y;
            this.X = x;
        }

        public Location Clone()
        {
            var newLocation = new Location
                (
                    this.Z,
                    this.Y,
                    this.X
                );

            return newLocation;
        }

        public bool Equals(Location otherLocation)
        {
            var output = (this.Z == otherLocation.Z && this.Y == otherLocation.Y && this.X == otherLocation.X);
            return output;
        }

        public bool IsValid(int maxZ, int maxY, int maxX)
        {
            return (this.Z >= 0 && this.Z < maxZ
                    && this.Y >= 0 && this.Y < maxY
                    && this.X >= 0 && this.X < maxX);
        }

        public override string ToString()
        {
            return $"location[z][y][x] : [{Z}][{Y}][{X}]";
        }

        // could create a Custom JsonConverter but this works, and I'm content with doing it the less efficient way
        // https://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm
        public void Deserialize(string str)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(str);
            var location = new Location(-1, -1, -1);
            if (dict.ContainsKey("Z") && dict.ContainsKey("Y") && dict.ContainsKey("X"))
            {
                Z = dict["Z"];
                Y = dict["Y"];
                Z = dict["Z"];
            }
        }

        public string Serialize()
        {
            var dict = new Dictionary<string, int>
            {
                { "Z", this.Z },
                { "Y", this.Y },
                { "X", this.X }
            };

            var str = JsonConvert.SerializeObject(dict);

            return str;
        }

    }
}