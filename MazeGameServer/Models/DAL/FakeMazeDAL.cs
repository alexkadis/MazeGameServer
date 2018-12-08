using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public class FakeMazeDAL : IMazeDAL
    {
        //private Dictionary<int, MazeTemplate> mazeTemplates = new Dictionary<int, MazeTemplate>()
        //{
        //    {
        //        1, new MazeTemplate
        //        {

        //        }
        //    }
        //};

        public Maze CreateMaze(Maze maze)
        {
            throw new NotImplementedException();
        }

        public void DeleteMaze(int id)
        {
            throw new NotImplementedException();
        }

        public Maze GetMaze(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Maze> GetMazes()
        {
            throw new NotImplementedException();
        }

        public Maze UpdateMaze(int id, Maze maze)
        {
            throw new NotImplementedException();
        }
    }
}
