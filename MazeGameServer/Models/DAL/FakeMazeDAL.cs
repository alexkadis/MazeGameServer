using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
	public class FakeMazeDAL : IMazeTemplateDAL
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
					bestPath: "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD",
					mazeDifficulty: 64
			)},
			{1, new MazeTemplate (
					startLocation: new Location (0, 0, 0),
					endLocation: new Location (0, 3, 1),
					gridLayers: 4,
					gridHeight: 8,
					gridWidth: 8,
					mazePath: "ESSDWDEEDDSDWDWDNDNDNDSENEDSWWNEBBBSSSDSSENDWNDSSDENWWSDSEDNWSDNNNDDNDSSUBBBBEBBBBBBBEDEDENEENNWSDENENNEUSDSUSWSEDDNWSSEDWDEDWWNDNNNWSWSEDWSESWBDDNDWWSEEEDNEENNNNWDNEDDSWWNDDNDEDDSENDWBDDBBBBBBWWDSDNWWWSEEDWBENEBWDSESSDENWWDSUSDENENNEDBBBWDWSWSDENNBWBBEENNBESDNBBSWBBBBBBBBDDBBBSBBBBBBBBBBBSUBSWSEBBBBBBBBBWBBNNWSBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBSESWBBBBBBBBBBBBBBBBBBBDBBBBBBBBBBBBBBBBBBBBBBBBBBENEBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBWBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
					bestPath: "3333333333333333",
					mazeDifficulty: 33
			)}

		};

		//private readonly MazeTemplate invalidMazeTemplate = new MazeTemplate
		//(
		//	startLocation: null,
		//	endLocation: null,
		//	gridLayers: -1,
		//	gridHeight: -1,
		//	gridWidth: -1,
		//	mazePath: null,
		//	bestPath: null,
		//	mazeDifficulty: -1
		//);

		public MazeTemplate GetMaze(int mazeId)
		{
			return mazeTemplates.GetValueOrDefault(mazeId);
		}

		public IList<MazeTemplate> GetAllMazeTemplatesInRange(int lowestMazeTemplateId = 0, int numberOfMazes = 50)
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
			var valid = mazeTemplate.IsValid();
			var exists = MazeExists(mazeTemplate);
			if (valid && !exists)
			{
				int nextId = mazeTemplates.Keys.ToList().OrderByDescending(i => i).First() + 1;
				mazeTemplate.MazeId = nextId;
				mazeTemplates[nextId] = mazeTemplate;
				return mazeTemplate;
			}
			return null;
		}

		// checks to see if the maze is already in our database
		public bool MazeExists(MazeTemplate mazeTemplate)
		{
			return mazeTemplates.ContainsKey(mazeTemplate.MazeId)
				&& mazeTemplate.Equals(mazeTemplates[mazeTemplate.MazeId]);
		}

		public MazeTemplate UpdateMaze(int mazeId, MazeTemplate mazeTemplate)
		{
			if(mazeTemplate.IsValid())
			{
				mazeTemplate.SetBestPath();

				if (mazeTemplates.ContainsKey(mazeId))
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
