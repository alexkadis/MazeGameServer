using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MazeGameServer.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace MazeGameServerTests
{
    [TestClass]
    public class MazeNavigatorTests
    {
        [TestMethod]
        public void CreateMazeAndDetermineDifficulty()
        {
            var exampleCompressed = "N4IgsghgXgpgChALgCxALhAUQMq4Kp4ByA6gCLZ7bnaaZHHFE5OGGnVN51eYnmaleDUoVqE8DYtkZtBAugCFyZWkUoUpVHI2YayknbxKjWRzAsoTSMglcZdpNaetxzBXNjTUkpKk70ocCiEGIndXMgVsBRjaZ3M8BUIYlJjE2IFokXYSVLylMmoY0wkuFOj8yqq8xlT9GiU0lPTq1rzWAnlSKOIotrbzatpW2or+8YnJqemZypAAGhBsRAgAJ0QAGQB7AGMkAEstgDt0EGAAHRAALUu0AAZ5y4BNW4fLgA1XgF8FrCOAE22e0QhxOGAu11ejxAL3QAHZoZ90ABmH6LBQwADOiAQKFOv0gsFI+wAZiT9jsAK4AG0QAE90A8QABxVb7f7Edl4tAADkWrPZAAkYPsAObIRDoPkstmAiB0mCrTHoAAsXyAA";

            Maze maze = new Maze(0, 0, 0, exampleCompressed);
            maze.DetermineMazeDifficulty(100);
            var difficulty1 = maze.BestPath;
            maze.DetermineMazeDifficulty(1);
            Assert.AreNotEqual(difficulty1, maze.BestPath);
        }

        [TestMethod]
        public void NavigatorCanTestSolution()
        {
            var compressed = "N4IgsghgXgpgChALgCxALhAZQKoFEByA6nrrpjqTtsdprrQCJnb4sMOsGFPk476tMhAflwdsbdkJaFyDQrNnUpOYRMYMKTUniZMJxNRKK1Z8g+YBC1y6Qa3LJ4WPzs7Q8UYXz2o-SVY8GW4xMnJLQmsCaktsS0wbG3lMdm4hIhSyenJaGWMaOMcBei4mCJtaRKqbOjtrVkT7asdqpuabXHau7vb8Hv7msVtKgdGxxKzC8fa26bn+kAAaLEQIACdEABkAewBjJABLbYA7dBBgAB0QAC0rtAAGRauATTvHq4ANN4BfJZBcY4AEx2+0QR1OGEuNzeTxAr3QAHZYV90ABWX7LSwwADOiAQKDOf0gsAYBwAZmSDrsAK4AG0QAE90I8QABxNYHQGETkEtAADmW7M5AAkYAcAObIRDoAVsjnAiAMmBrbHoAAs3yAA";

            Maze maze = new Maze(0, 0, 0, compressed);
            var valdSolution = "DUSUDUENSUSNDUSEWUUWUEDUSUWEDUWDUDDDDUSDDUUUDDDUESUUDSDEWEUUNDUUSEWEUUENSUSE";

            MazeNavigator navigator = new MazeNavigator(maze);
            Assert.IsTrue(navigator.IsNavigatablePath(valdSolution));

        }

        [TestMethod]
        public void NavigatorCanSolveMaze()
        {
            // random maze
            Maze maze = new Maze(4, 8, 8);
            maze.DetermineMazeDifficulty(1);

            MazeNavigator navigator = new MazeNavigator(maze);
            Assert.IsTrue(navigator.IsNavigatablePath(maze.BestPath));

        }


        [TestMethod]
        public void RecreateMazeRandomMazeProcedurally()
        {
            Maze maze = new Maze(4, 8, 8);
            Utils utils = new Utils();
            Maze maze2 = new Maze(0, 0, 0, maze.MazeTemplateCompressed);
            Assert.AreEqual(maze.MazePath, maze2.MazePath);
        }
    }
}
