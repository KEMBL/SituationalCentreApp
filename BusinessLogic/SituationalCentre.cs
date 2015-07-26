using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class SituationalCentre
    {
        private readonly IEventsProvider eventsProvider;
        private readonly IDataStorage dataStorage;

        public SituationalCentre(IEventsProvider eventsProvider, IDataStorage dataStorage)
        {
            // TODO: Complete member initialization
            this.eventsProvider = eventsProvider;
            this.dataStorage = dataStorage;
        }
    }
}
