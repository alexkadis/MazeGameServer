namespace MazeGameServer.Models
{
    public class Location
    {
        public int Z { get; set; }
        public int Y { get; set; }
        public int X { get; set; }

        public Location Clone()
        {
            var newLocation = new Location();
            newLocation.Z = this.Z;
            newLocation.Y = this.Y;
            newLocation.X = this.X;
            return newLocation;
        }


    }
}