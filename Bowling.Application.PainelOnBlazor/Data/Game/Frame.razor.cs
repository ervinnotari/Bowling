using Bowling.Domain.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Web
{
    public class FrameBase : ComponentBase
    {
        [Parameter] public Player Player { get; set; }
        [Parameter] public int FrameId { get; set; }
        protected Frame Frame => Player.Frames.ElementAtOrDefault(FrameId);
        protected string Attempt1 => AttNormalize(Frame.Balls.ElementAtOrDefault(0), 1);
        protected string Attempt2 => (Frame.Balls.Count > 1) ? AttNormalize(Frame.Balls.ElementAtOrDefault(1), 2) : null;
        protected string Attempt3 => (Frame.Balls.Count > 2) ? AttNormalize(Frame.Balls.ElementAtOrDefault(2), 3) : null;
        protected string Score => Frame?.Score.ToString();

        private string AttNormalize(int? value, int id)
        {
            if (value == 10) return "X";
            else if (value == 0) return "-";
            else if ((id == 2 && Frame.Balls.ElementAtOrDefault(1) + value == 10) || (id == 3 && Frame.Balls.ElementAtOrDefault(2) + value == 10)) return "/";
            else return $"{value}";
        }
    }
}
