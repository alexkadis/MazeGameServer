using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public interface IMazeDAL
    {
        IList<Maze> GetMazes();
        Maze GetMaze(int id);
        Maze CreateMaze(Maze maze);
        Maze UpdateMaze(int id, Maze maze);
        void DeleteMaze(int id);
    }
}
