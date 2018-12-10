using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MazeGameServer.Models;

namespace MazeGameServerTests
{
    [TestClass]
    public class UtilsTests
    {
   //     [TestMethod]
   //     public void UncompressTemplateShouldReturnProperlyUncompressedMaze()
   //     {
			//var exampleCompressed = "N4IgsghgXgpgChALgCxALhAUQMq4Kp4ByA6gCLZ7bnaaZHHFE5OGGnVN51eYnmaleDUoVqE8DYtkZtBAugCFyZWkUoUpVHI2YayknbxKjWRzAsoTSMglcZdpNaetxzBXNjTUkpKk70ocCiEGIndXMgVsBRjaZ3M8BUIYlJjE2IFokXYSVLylMmoY0wkuFOj8yqq8xlT9GiU0lPTq1rzWAnlSKOIotrbzatpW2or+8YnJqemZypAAGhBsRAgAJ0QAGQB7AGMkAEstgDt0EGAAHRAALUu0AAZ5y4BNW4fLgA1XgF8FrCOAE22e0QhxOGAu11ejxAL3QAHZoZ90ABmH6LBQwADOiAQKFOv0gsFI+wAZiT9jsAK4AG0QAE90A8QABxVb7f7Edl4tAADkWrPZAAkYPsAObIRDoPkstmAiB0mCrTHoAAsXyAA";
   //         var exampleUncompressedMazePath = "ESSSUUNWDSUSDSSEEUNWWUNESUNENNDDSSUNEUEUUEENWDSEDENWWDNEENUWWWSWUNDDEEDEUBDSWDEEUNUSSUWSSDESWUESUSUWWDWWWWUEENNWNENNENNEBUSUWDWUNUUUWDWUUESWSESWUSSSSDEEDEUENDSEUNUNWWSWDENENENUSESSUENWWWUNDEUSSDWDBSBBBEESWUEBUBNBBBBBBBUBBBEEDSBDNDDNWBBBBBBBBBBDWDDSSBBNNEUWUEBBBSBBBBBBBBBBBBBBBBBBBBBBBBBBBWUBBBBWDWSEBDBBUBBBUBBBBBBBBBBBBBBBBBBBBBBBBBBNNUUEDEDBSWBSBBBBBBBBBBBBBBBBBBBBBBEBBBBBBBBBBEEBBBBBBBBBBWUBSBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";


   //         Utils utils = new Utils();
   //         //var uncompressedMaze = utils.UncompressTemplate(exampleCompressed);

			////Assert.AreEqual(exampleUncompressedMazePath, uncompressedMaze.MazePath);
   //     }

		//[TestMethod]
		//public void CompressTemplateShouldReturnProplerlyCompressedMaze()
		//{
  //          var exampleCompressed = "N4IgsghgXgpgChALgCxALhAUQMq4Kp4ByA6gCLZ7bnaaZHHFE5OGGnVN51eYnmaleDUoVqE8DYtkZtBAugCFyZWkUoUpVHI2YayknbxKjWRzAsoTSMglcZdpNaetxzBXNjTUkpKk70ocCiEGIndXMgVsBRjaZ3M8BUIYlJjE2IFokXYSVLylMmoY0wkuFOj8yqq8xlT9GiU0lPTq1rzWAnlSKOIotrbzatpW2or+8YnJqemZypAAGhBsRAgAJ0QAGQB7AGMkAEstgDt0EGAAHRAALUu0AAZ5y4BNW4fLgA1XgF8FrCOAE22e0QhxOGAu11ejxAL3QAHZoZ90ABmH6LBQwADOiAQKFOv0gsFI+wAZiT9jsAK4AG0QAE90A8QABxVb7f7Edl4tAADkWrPZAAkYPsAObIRDoPkstmAiB0mCrTHoAAsXyAA";
  //          Maze maze = new Maze(exampleCompressed);
		//	Utils utils = new Utils();
		//	//var compressedMaze = utils.CompressTemplate(maze);
  //          //var two = utils.CompressTemplate(utils.UncompressTemplate(compressedMaze));

  //          //Assert.AreEqual(compressedMaze, two);
		//}

		[TestMethod]
		public void LocationIsValidReturnsValidResults()
		{
			var maxZ = 3;
			var maxY = 7;
			var maxX = 7;

			var location = new Location(0, 1, 1);

			Assert.IsTrue(location.IsValid(maxZ, maxY, maxX));
		}


    }
}
