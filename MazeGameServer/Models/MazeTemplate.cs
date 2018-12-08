using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models
{
    public class MazeTemplate
    {
        public Location StartLocation { get; private set; }
        public Location EndLocation { get; private set; }
        public int GridLayers { get; private set; }
        public int GridHeight { get; private set; }
        public int GridWidth { get; private set; }
        public string MazePath { get; set; }
        public string BestPath { get; set; }
        public int MazeDifficulty { get; set; }

        public MazeTemplate(Location startLocation, Location endLocation, int gridLayers, int gridHeight, int gridWidth, string mazePath = null, string bestPath = null, int mazeDifficulty = -1)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            GridLayers = gridLayers;
            GridHeight = gridHeight;
            GridWidth = gridWidth;
            MazePath = mazePath;
            BestPath = bestPath;
            MazeDifficulty = mazeDifficulty;
        }

        public MazeTemplate(string mazeTemplateCompressed)
        {
            this.Uncompress(mazeTemplateCompressed);
        }

        public void Uncompress(string mazeTemplateCompressed)
        {
            try
            {
                string str = LZString.DecompressFromEncodedURIComponent(mazeTemplateCompressed);
                var templateAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

                this.MazePath = templateAsDictionary["MazePath"];
                this.StartLocation = new Location(templateAsDictionary["StartLocation"]);
                this.EndLocation = new Location(templateAsDictionary["EndLocation"]);
                this.BestPath = templateAsDictionary["BestPath"];
                this.MazeDifficulty = Convert.ToInt32(templateAsDictionary["MazeDifficulty"]);
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

        public string Compress(bool encode = false)
        {
            Dictionary<string, object> template = new Dictionary<string, object>
            {
                { "MazePath", this.MazePath },
                { "StartLocation", this.StartLocation},
                { "EndLocation", this.EndLocation},
                { "BestPath", this.BestPath},
                { "MazeDifficulty", this.MazeDifficulty},
                { "GridWidth", this.GridWidth},
                { "GridHeight", this.GridHeight},
                { "GridLayers", this.GridLayers}
            };
            
            var compressed = JsonConvert.SerializeObject(template);
            if (encode)
            {
                compressed = LZString.CompressToEncodedURIComponent(compressed);
            }
            return compressed;
        }

        public MazeTemplate Clone()
        {
            return new MazeTemplate(StartLocation.Clone(), EndLocation.Clone(), GridLayers, GridHeight, GridWidth, MazePath, BestPath, MazeDifficulty);
        }
    }
}
