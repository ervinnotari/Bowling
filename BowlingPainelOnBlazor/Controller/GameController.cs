using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BowlingPainelOnBlazor.Data;
using BowlingGame;

namespace BowlingPainelOnBlazor.Controller
{
    [ApiController]
    [Route("[controller]/{alley}")]
    public class GameController : ControllerBase
    {
        private readonly BowlingService BowlingService;

        public GameController(BowlingService bowlingService)
        {
            BowlingService = bowlingService;
        }

        [HttpDelete]
        [Route("")]
        public ActionResult Default(string alley)
        {
            BowlingService.Clear(alley);
            return this.NoContent();
        }

        [HttpGet]
        [Route("score")]
        public Painel Score(string alley)
        {
            return BowlingService.GetPainel(alley);
        }
    }
}
