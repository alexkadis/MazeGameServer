using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public interface IGameDAL
    {
		/// <summary>
		/// Gets all games.
		/// </summary>
		/// <returns>The all games.</returns>
        IList<Game> GetAllGames();

		/// <summary>
		/// Gets the game requested game.
		/// </summary>
		/// <returns>The game.</returns>
		/// <param name="gameId">Game identifier.</param>
        Game GetGame(int gameId);

		/// <summary>
		/// Gets the ranking that a particular game has with a maze
		/// </summary>
		/// <returns>The ranking.</returns>
		/// <param name="gameId">Game identifier.</param>
		int GetRanking(int gameId);

		/// <summary>
		/// Saves a game.
		/// </summary>
		/// <returns>The game.</returns>
		/// <param name="game">Game.</param>
        Game SaveGame(Game game);

		/// <summary>
		/// Deletes a game.
		/// </summary>
		/// <param name="gameId">Game identifier.</param>
        void DeleteGame(int gameId);
    }
}
