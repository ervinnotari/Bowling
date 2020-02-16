using BowlingGame;
using BowlingPainelOnBlazor.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace BowlingPainelOnBlazor.Controller
{
    /// <summary>
    /// The bowling game controll.
    /// </summary>
    [ApiController]
    [Route("game/{alley}")]
    public class GameController : ControllerBase
    {

        private readonly BowlingService BowlingService;

        public GameController(BowlingService bowlingService)
        {
            BowlingService = bowlingService;
        }

        /// <summary>
        /// Clear moves and restart the game in specific alley.
        /// </summary>
        /// <param name="alley">Alley specified for action</param> 
        /// <response code="204">Success and nothing more</response>
        /// <response code="417">The <paramref name="alley"/> not found</response>
        /// <response code="500">An unknown error occurred internally on server</response>
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public void ClearAlley(string alley)
        {
            if (BowlingService.Bowling.Scores.ContainsKey(alley)) BowlingService.Clear(alley);
            else throw new HttpListenerException(StatusCodes.Status417ExpectationFailed, "Alley not found");
        }

        /// <summary>
        /// Get score of plays from a specific alley the game. 
        /// </summary>
        /// <param name="alley">Alley specified for action</param>
        /// <returns>Returns current score of alley</returns>
        /// <response code="200">Success</response>       
        /// <response code="417">The <paramref name="alley"/> not found</response>
        /// <response code="500">An unknown error occurred internally on server</response>
        [HttpGet]
        [Route("score")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Painel GetScore(string alley)
        {
            if (BowlingService.Bowling.Scores.ContainsKey(alley)) return BowlingService.GetPainel(alley);
            else throw new HttpListenerException(StatusCodes.Status417ExpectationFailed, "Alley not found");
        }
    }
}
