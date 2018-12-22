using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeGameServer.Models;
using MazeGameServer.Models.DAL;
using Microsoft.AspNetCore.Mvc;

namespace MazeGameServer.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class ApiGamesController : ControllerBase
    {
        private IGameDAL GameDAL;
        private IMazeTemplateDAL MazeTemplateDAL;

        public ApiGamesController(IGameDAL gameDAL, IMazeTemplateDAL mazeTemplateDAL)
        {
            GameDAL = gameDAL;
            MazeTemplateDAL = mazeTemplateDAL;
        }


        [HttpGet]
        public ActionResult<List<Game>> GetGames()
        {
            return GameDAL.GetAllGames().ToList();
        }

        [HttpGet("{id?}", Name = "GetGame")]
        public ActionResult<Game> GetGame(int? id)
        {
            if (id != null)
            {
                var game = GameDAL.GetGame((int)id);

                if (game != null)
                {
                    return game;
                }
            }
            return NotFound();
        }

		[HttpGet("{id?}", Name = "GetRanking")]
		public ActionResult<Game> GetRanking(int? id)
		{
			if (id != null)
			{
				var game = GameDAL.GetGame((int)id);

				if (game != null)
				{
					return game;
				}
			}
			return NotFound();
		}



		[HttpPost]
		public ActionResult Create(Game game)
		{
			if (game.MazeTemplate.IsValid())
			{
				if (!MazeTemplateDAL.MazeExists(game.MazeTemplate))
				{
					MazeTemplateDAL.SaveMaze(game.MazeTemplate);
				}
				var newGame = GameDAL.SaveGame(game);
				return CreatedAtRoute("GatGame", new { id = newGame.GameId }, newGame);
			}
			return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity, "Invalid MazeTemplate");
		}

		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			var game = GameDAL.GetGame(id);

			if (game == null)
			{
				return NotFound();
			}

			GameDAL.DeleteGame(id);
			return NoContent();
		}
	}
}