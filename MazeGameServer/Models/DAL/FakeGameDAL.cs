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
                    PathTaken = "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD",
                    UserId = 0,
                    Username = "apple",
                    MazeTemplate = new MazeTemplate()
                    {
                        StartLocation = new Location (0, 0, 0),
                        EndLocation = new Location(0, 3, 1),
                        GridLayers = 4,
                        GridHeight = 8,
                        GridWidth = 8,
                        MazePath = "ESSDWDEEDDSDWDWDNDNDNDSENEDSWWNEBBBSSSDSSENDWNDSSDENWWSDSEDNWSDNNNDDNDSSUBBBBEBBBBBBBEDEDENEENNWSDENENNEUSDSUSWSEDDNWSSEDWDEDWWNDNNNWSWSEDWSESWBDDNDWWSEEEDNEENNNNWDNEDDSWWNDDNDEDDSENDWBDDBBBBBBWWDSDNWWWSEEDWBENEBWDSESSDENWWDSUSDENENNEDBBBWDWSWSDENNBWBBEENNBESDNBBSWBBBBBBBBDDBBBSBBBBBBBBBBBSUBSWSEBBBBBBBBBWBBNNWSBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBSESWBBBBBBBBBBBBBBBBBBBDBBBBBBBBBBBBBBBBBBBBBBBBBBENEBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBWBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
                        BestPath = "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD"
                    }
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
                int nextId = Games.Keys.ToList().OrderByDescending(i => i).First() + 1;
                game.GameId = nextId;
                Games[nextId] = game;
                return game;
        }

        void IGameDAL.DeleteGame(int gameId)
        {
			Games.Remove(gameId);
		}

		public int GetRanking(int gameId)
		{
			throw new NotImplementedException();
		}
	}
}
