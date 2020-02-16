using BowlingGame;
using BowlingPainelOnBlazor.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BowlingPainelOnBlazor.Controller
{
    /// <summary>
    /// The bowling game controll.
    /// </summary>
    [ApiController]
    [Route("game")]
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
        /// <response code="417">A problem in performing the requested action caused by some reported data</response>
        /// <response code="500">An unknown error occurred internally on server</response>
        [HttpDelete]
        [Route("{alley}")]
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
        /// <response code="417">A problem in performing the requested action caused by some reported data</response>
        /// <response code="500">An unknown error occurred internally on server</response>
        [HttpGet]
        [Route("{alley}/score")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Painel GetScore(string alley)
        {
            if (BowlingService.Bowling.Scores.ContainsKey(alley)) return BowlingService.GetPainel(alley);
            else throw new HttpListenerException(StatusCodes.Status417ExpectationFailed, "Alley not found");
        }

        /// <summary>
        /// Make play. 
        /// </summary>
        /// <param name="play">Play specified for action</param>
        /// <response code="201">Successfully created</response>       
        /// <response code="417">A problem in performing the requested action caused by some reported data</response>
        /// <response code="500">An unknown error occurred internally on server</response>
        [HttpPost]
        [Route("play")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public void MakePlay(Play play)
        {
            try
            {
                BowlingService.AddPlay(play);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new HttpListenerException(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public class Play : BowlingGame.Play
        {
            [Required]
            new public string Name
            {
                get { return base.Name; }
                set { base.Name = value; }
            }
            [Required]
            new public string Alley
            {
                get { return base.Alley; }
                set { base.Alley = value; }
            }
            [Required]
            [Range(0, 10)]
            new public int Pins
            {
                get { return base.Pins; }
                set { base.Pins = value; }
            }
            [Required]
            new public DateTime Date
            {
                get { return base.Date; }
                set { base.Date = value; }
            }
            public Play() : base("", 0, "", DateTime.Now) { }
        }
    }
}
