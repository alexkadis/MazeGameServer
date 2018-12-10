using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeGameServer.Models;
using MazeGameServer.Models.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace MazeGameServer.Controllers
{
	[Route("api/mazes")]
	[ApiController]
	public class ApiMazesController : ControllerBase
	{
		// http://book.techelevator.com/.net/70-javascript/55-create-rest-api/05-dotnet.html

		private IMazeTemplateDAL dal;

		public ApiMazesController(IMazeTemplateDAL dal)
		{
			this.dal = dal;
		}

        [HttpGet]
		public ActionResult<List<MazeTemplate>> GetMazes()
		{
			return dal.GetAllMazeTemplates().ToList();
		}

		[HttpGet("{id?}", Name = "GetMaze")]
		public ActionResult<MazeTemplate> GetMaze(int? id)
		{
			if (id != null)
			{
				var mazeTemplate = dal.GetMaze((int)id);

				if (mazeTemplate != null)
				{
					return mazeTemplate;
				}
			}
			return NotFound();
		}

		[HttpGet("random/{count?}/{d?}/{dimensions?}", Name = "Random")]
		public ActionResult<List<MazeTemplate>> Random(int count = 1, string d = null, string dimensions = null)
		{
			int layers = 4;
			int height = 8;
			int width = 8;
			var mazeTemplates = new List<MazeTemplate>();
			if (!string.IsNullOrEmpty(dimensions) && d == "dimensions")
			{
				var dims = (string)dimensions;
				var dimensionsArray = dims.Split(':');
				if (dimensionsArray.Length == 3)
				{
					if (int.TryParse(dimensionsArray[0], out int l))
					{
						layers = l;
					}
					if (int.TryParse(dimensionsArray[1], out int h))
					{
						height = h;
					}
					if (int.TryParse(dimensionsArray[2], out int w))
					{
						width = w;
					}
				}
			}

			var utilities = new Utils();
			count = utilities.ForceNumberToBeWithinRange(count, 1, 100);

			for (int i = 0; i < count; i++)
			{
				mazeTemplates.Add(dal.GenerateRandomMaze(layers, height, width));
			}

			return mazeTemplates;
		}

		[HttpPost]
		public ActionResult Create(MazeTemplate mazeTemplate)
		{
			var newMazeTemplate = dal.SaveMaze(mazeTemplate);

			return CreatedAtRoute("GetMaze", new { id = mazeTemplate.MazeId }, newMazeTemplate);
		}


		[HttpPut("{id}")]
		public ActionResult Update(int id, MazeTemplate mazeTemplate)
		{
			var existingMaze = dal.GetMaze(id);
			if (existingMaze == null)
			{
				return NotFound();
			}
            try
            {
                dal.UpdateMaze(id, mazeTemplate);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid Direction in Path")
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity,"Given bestPath is not a possible solution to the maze.");
                }
            }

            return NoContent();
        }

		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
            var mazeId = (int)id;
			var mazeTemplate = dal.GetMaze(mazeId);

			if (mazeTemplate == null)
			{
				return NotFound();
			}

			dal.DeleteMaze(mazeId);
			return NoContent();
		}

	}
}