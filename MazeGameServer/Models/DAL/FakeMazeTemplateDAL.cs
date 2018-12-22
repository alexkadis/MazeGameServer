using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
	public class FakeMazeTemplateDAL : IMazeTemplateDAL
	{
		private readonly Dictionary<int, MazeTemplate> mazeTemplates = new Dictionary<int, MazeTemplate>()
		{
			{0, new MazeTemplate (
					startLocation: new Location (0, 0, 0),
					endLocation: new Location (0, 3, 1),
					gridLayers: 4,
					gridHeight: 8,
					gridWidth: 8,
					mazePath: "ESSDWDEEDDSDWDWDNDNDNDSENEDSWWNEBBBSSSDSSENDWNDSSDENWWSDSEDNWSDNNNDDNDSSUBBBBEBBBBBBBEDEDENEENNWSDENENNEUSDSUSWSEDDNWSSEDWDEDWWNDNNNWSWSEDWSESWBDDNDWWSEEEDNEENNNNWDNEDDSWWNDDNDEDDSENDWBDDBBBBBBWWDSDNWWWSEEDWBENEBWDSESSDENWWDSUSDENENNEDBBBWDWSWSDENNBWBBEENNBESDNBBSWBBBBBBBBDDBBBSBBBBBBBBBBBSUBSWSEBBBBBBBBBWBBNNWSBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBSESWBBBBBBBBBBBBBBBBBBBDBBBBBBBBBBBBBBBBBBBBBBBBBBENEBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBWBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
					bestPath: "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD"
			)}
		};

		public MazeTemplate GetMaze(int mazeId)
		{
			return mazeTemplates.GetValueOrDefault(mazeId);
		}

		public IList<MazeTemplate> GetAllMazeTemplates() //InRange(int lowestMazeTemplateId = 0, int numberOfMazes = 50)
		{
			// just currently returns all of the mazes... would have to do a limit clause for a DB
			return mazeTemplates.Values.ToList();
		}

		public MazeTemplate GenerateRandomMaze(int z, int y, int x)
		{
			var maze = new Maze(z, y, x);
			var mazeTemplate = maze.Template;
			mazeTemplate.DetermineMazeDifficulty(maze);
			SaveMaze(mazeTemplate);
			return mazeTemplate;
		}

		public MazeTemplate SaveMaze(MazeTemplate mazeTemplate)
		{
			int nextId = mazeTemplates.Keys.ToList().OrderByDescending(i => i).First() + 1;
			mazeTemplate.MazeId = nextId;
			mazeTemplates[nextId] = mazeTemplate;
			return mazeTemplate;
		}

		// checks to see if the maze is already in our database
		public bool MazeExists(MazeTemplate mazeTemplate)
		{
			return mazeTemplates.ContainsKey(mazeTemplate.MazeId)
				&& mazeTemplate.Equals(mazeTemplates[mazeTemplate.MazeId]);
		}

        public bool MazeExists(int id)
        {
            return mazeTemplates.ContainsKey(id);
        }

        public MazeTemplate UpdateMaze(int mazeId, MazeTemplate mazeTemplate)
		{
			if(mazeTemplate.IsValid())
			{
				mazeTemplate.SetBestPath();

				if (mazeTemplates.ContainsKey(mazeTemplate.MazeId))
				{
                    mazeTemplate.MazeId = mazeId;
                    mazeTemplates[mazeId] = mazeTemplate;
				}

				return mazeTemplate;
			}
			return null;
		}

		public void DeleteMaze(int mazeId)
		{
			mazeTemplates.Remove(mazeId);
		}
	}
}
