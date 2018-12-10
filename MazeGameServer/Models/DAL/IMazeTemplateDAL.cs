using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public interface IMazeTemplateDAL
	{
        /// <summary>
        /// Gets all maze templates within a range of IDs.
        /// </summary>
        /// <returns>The maze templates in range.</returns>
        //// <param name="lowestMazeTemplateId">Lowest maze template identifier.</param>
        //// <param name="numberOfMazes">Number of mazes to return. Maximum is 50.</param>
        IList<MazeTemplate> GetAllMazeTemplates(); // sInRange(int lowestMazeTemplateId = 0, int numberOfMazes = 50); // maximum number of mazes is 50

		/// <summary>
		/// Generates a random maze.
		/// </summary>
		/// <returns>The random maze.</returns>
		MazeTemplate GenerateRandomMaze(int z = 4, int y = 8, int x = 8);

		/// <summary>
		/// Gets a maze given a maze ID
		/// </summary>
		/// <returns>The maze.</returns>
		/// <param name="mazeId">Maze identifier.</param>
		MazeTemplate GetMaze(int mazeId);

		/// <summary>
		/// Given a MazeTemplate:
		/// Checks to make sure the maze is valid, and if so it saves it
		/// </summary>
		/// <returns>The maze with the new official ID</returns>
		/// <param name="mazeTemplate">Maze template.</param>
		MazeTemplate SaveMaze(MazeTemplate mazeTemplate);

		/// <summary>
		/// Updates a given maze, likely with a new solution
		/// </summary>
		/// <returns>The maze.</returns>
		/// <param name="mazeId">Maze identifier.</param>
		/// <param name="mazeTemplate">Maze template.</param>
		MazeTemplate UpdateMaze(int mazeId, MazeTemplate mazeTemplate);

		/// <summary>
		/// Deletes a maze.
		/// </summary>
		/// <param name="mazeId">Maze identifier.</param>
        void DeleteMaze(int mazeId);
    }
}
