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
        private IGameDAL dal;

        public ApiGamesController(IGameDAL dal)
        {
            this.dal = dal;
        }


        [HttpGet]
        public ActionResult<List<Game>> GetGames()
        {
            return dal.GetAllGames().ToList();
        }

        [HttpGet("{id?}", Name = "GetGame")]
        public ActionResult<Game> GetGame(int? id)
        {
            if (id != null)
            {
                var game = dal.GetGame((int)id);

                if (game != null)
                {
                    return game;
                }
            }
            return NotFound();
        }
    }
}