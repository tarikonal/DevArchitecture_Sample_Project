
using Business.Handlers.Games.Commands;
using Business.Handlers.Games.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using Core.Entities.Concrete;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Games If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
   // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GamesController : BaseApiController
    {
        ///<summary>
        ///List Games
        ///</summary>
        ///<remarks>Games</remarks>
        ///<return>List Games</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Game>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        //[HttpGet("getall")]
        [HttpGet]

        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetGamesQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Translate>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("dtos")]
        public async Task<IActionResult> GetGameListDto()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetGamesListDtoQuery()));
        }


        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>Games</remarks>
        ///<return>Games List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new GetGameQuery { Id = id });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Game.
        /// </summary>
        /// <param name="createGame"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGameCommand createGame)
        {
            var result = await Mediator.Send(createGame);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update Game.
        /// </summary>
        /// <param name="updateGame"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGameCommand updateGame)
        {
            var result = await Mediator.Send(updateGame);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Game.
        /// </summary>
        /// <param name="deleteGame"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteGameCommand deleteGame)
        {
            var result = await Mediator.Send(deleteGame);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
