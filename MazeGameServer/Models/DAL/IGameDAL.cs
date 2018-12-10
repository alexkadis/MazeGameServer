using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public interface IGameDAL
    {
        IList<Game> GetAllGames();

        Game GetGame(int gameId);

        Game SaveGame(Game game);

        Game UpdateGame(int gameId, Game game);

        void DeleteGame(int gameId);
    }
}
