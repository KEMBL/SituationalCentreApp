using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLogics;

namespace XMLDataStorageLib
{
    /// <summary>
    /// This system get and store data from XML files. Each XML file is a data table.
    /// </summary>
    public class XMLDataStorage : IDataStorage
    {
        /// <summary>
        /// State of the current component
        /// </summary>
        public ComponentState State { get; set; }
        /// <summary>
        /// All information initialized and system ready to work
        /// </summary>
        public bool StoreLoaded {
            get { return State == ComponentState.Ready; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public XMLDataStorage()
        {
            //all works fine            
            State = ComponentState.Ready;
        }

        public void Dispose()
        {
            
        }

    }
}
