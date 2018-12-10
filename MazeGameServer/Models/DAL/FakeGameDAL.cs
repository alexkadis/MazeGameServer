using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGameServer.Models.DAL
{
    public class FakeGameDAL : IGameDAL
    {

        private readonly Dictionary<int, Game> Games = new Dictionary<int, Game>()
        {
            { 0, new Game
                {
                    GameDate = new DateTime(),
                    MazeId = 0,
                    PathTaken = "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD",
                    UserId = 0,
                    Username = "apple"
                }
            },
            { 1, new Game
                {
                    GameDate = new DateTime(),
                    MazeId = 1,
                    PathTaken = "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD",
                    UserId = 0,
                    Username = "apple"
                }
            }
        };

        IList<Game> IGameDAL.GetAllGames()
        {
            return Games.Values.ToList();
        }

        Game IGameDAL.GetGame(int gameId)
        {
            return Games.GetValueOrDefault(gameId);
        }

        Game IGameDAL.SaveGame(Game game)
        {
            //var exists = MazeExists(game.MazeId);
            //if (valid && !exists)
            //{
            //    int nextId = mazeTemplates.Keys.ToList().OrderByDescending(i => i).First() + 1;
            //    mazeTemplate.MazeId = nextId;
            //    mazeTemplates[nextId] = mazeTemplate;
            //    return mazeTemplate;
            //}
            return null;
        }

        Game IGameDAL.UpdateGame(int gameId, Game game)
        {
            throw new NotImplementedException();
        }

        void IGameDAL.DeleteGame(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
