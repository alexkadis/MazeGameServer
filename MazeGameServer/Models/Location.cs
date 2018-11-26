namespace MazeGameServer.Models
{
    public class Location
    {
        public int Z { get; set; }
        public int Y { get; set; }
        public int X { get; set; }

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
    }
}