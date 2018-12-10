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
        //// this test is kinda useless 
        //[TestMethod]
        //public void CreateMazeAndDetermineDifficulty()
        //{
        //    var exampleCompressed = "N4IgsghgXgpgChALgCxALhAUQMq4Kp4ByA6gCLZ7bnaaZHHFE5OGGnVN51eYnmaleDUoVqE8DYtkZtBAugCFyZWkUoUpVHI2YayknbxKjWRzAsoTSMglcZdpNaetxzBXNjTUkpKk70ocCiEGIndXMgVsBRjaZ3M8BUIYlJjE2IFokXYSVLylMmoY0wkuFOj8yqq8xlT9GiU0lPTq1rzWAnlSKOIotrbzatpW2or+8YnJqemZypAAGhBsRAgAJ0QAGQB7AGMkAEstgDt0EGAAHRAALUu0AAZ5y4BNW4fLgA1XgF8FrCOAE22e0QhxOGAu11ejxAL3QAHZoZ90ABmH6LBQwADOiAQKFOv0gsFI+wAZiT9jsAK4AG0QAE90A8QABxVb7f7Edl4tAADkWrPZAAkYPsAObIRDoPkstmAiB0mCrTHoAAsXyAA";

        //    Maze maze = new Maze(0, 0, 0, exampleCompressed);
        //    maze.DetermineMazeDifficulty(100);
        //    var difficulty1 = maze.BestPath;
        //    maze.DetermineMazeDifficulty(1);
        //    Assert.AreNotEqual(difficulty1, maze.BestPath);
        //}

        [TestMethod]
        public void NavigatorCanTestSolution()
        {
            var template = new MazeTemplate(
                    startLocation: new Location(0, 0, 0),
                    endLocation: new Location(0, 3, 1),
                    gridLayers: 4,
                    gridHeight: 8,
                    gridWidth: 8,
                    mazePath: "ESSDWDEEDDSDWDWDNDNDNDSENEDSWWNEBBBSSSDSSENDWNDSSDENWWSDSEDNWSDNNNDDNDSSUBBBBEBBBBBBBEDEDENEENNWSDENENNEUSDSUSWSEDDNWSSEDWDEDWWNDNNNWSWSEDWSESWBDDNDWWSEEEDNEENNNNWDNEDDSWWNDDNDEDDSENDWBDDBBBBBBWWDSDNWWWSEEDWBENEBWDSESSDENWWDSUSDENENNEDBBBWDWSWSDENNBWBBEENNBESDNBBSWBBBBBBBBDDBBBSBBBBBBBBBBBSUBSWSEBBBBBBBBBWBBNNWSBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBSESWBBBBBBBBBBBBBBBBBBBDBBBBBBBBBBBBBBBBBBBBBBBBBBENEBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBWBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
                    bestPath: "UDUUUUEDSDUEUDUDEWWNUUUUUWUUUSDDUEUNEUUUWUUUWSUNDSUNDUDSDUESSUDD",
                    mazeDifficulty: 64
            );
            
            Maze maze = new Maze(template);
            MazeNavigator navigator = new MazeNavigator(maze);
            Assert.IsTrue(navigator.IsNavigatablePath(template.BestPath));
        }

        [TestMethod]
        public void NavigatorCanSolveMaze()
        {
            // random maze
            Maze maze = new Maze(4, 8, 8);
            maze.Template.DetermineMazeDifficulty(maze, 3000, 50);

            MazeNavigator navigator = new MazeNavigator(maze);
            Assert.IsTrue(navigator.IsNavigatablePath(maze.Template.BestPath));
            //var wait = false;

        }


        [TestMethod]
        public void RecreateMazeRandomMazeProcedurally()
        {
            Maze maze = new Maze(4, 8, 8);
            Utils utils = new Utils();
            Maze maze2 = new Maze(maze.Template.Clone());
            Assert.AreEqual(maze.Template.MazePath, maze2.Template.MazePath);
        }
    }
}
