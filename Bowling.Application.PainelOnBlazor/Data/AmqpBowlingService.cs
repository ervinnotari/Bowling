using Bowling.Service.NMS;
using Microsoft.Extensions.Options;

namespace BowlingPainelOnBlazor.Data
{

    public class AmqpBowlingService : NmsService, IService
    {
        public AmqpBowlingService(IOptions<NmsConfigurations> o)
        {
            Configurations = o.Value;
        }
    }

}
