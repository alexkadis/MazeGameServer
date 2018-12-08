using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MazeGameServer.Models;

namespace MazeGameServer.Controllers
{
    public class HomeController : Controller
    {
        Utils Utilities { get; }
        //LZString lzString { get;  }

        public HomeController()
        {
            this.Utilities = new Utils();
            //this.lzString = new LZString();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Generate(int z, int y, int x, int n = 1, bool determineDifficulty = true)
        {

            var json = CreateJSON(z, y, x, n, determineDifficulty);


            return Content(json, "application / json");
        }

        private string CreateJSON(int z, int y, int x, int n, bool determineDifficulty = true)
        {
            string json = String.Empty;

            for (int i = 0; i < n; i++)
            {
                json += CreateMaze(z, y, x, determineDifficulty);
                if (i != n - 1)
                {
                    json += ",";
                }
            }
            return "{\"mazes\": [" + json  + "]}";
        }


        private string CreateMaze(int z, int y, int x, bool determineDifficulty = true)
        {
            Maze maze = new Maze(z, y, x);
            if (determineDifficulty)
            {
                //maze.DetermineMazeDifficulty(5000, 100, 1000);
            }
            var template = maze.Template.Compress();
            //template = $"\"{maze.MazeDifficulty}\": {template}";

            return template;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
