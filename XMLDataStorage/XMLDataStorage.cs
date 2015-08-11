using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDataStorageLib
{
    /// <summary>
    /// This system get and store data from XML files. Each XML file is a data table.
    /// </summary>
    public class XMLDataStorage : IDataStorage
    {
        /// <summary>
        /// Current storage system state
        /// </summary>
        private bool _storeState;
        /// <summary>
        /// All information initialized and system ready to work
        /// </summary>
        public bool StoreLoaded {
            get { return _storeState; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public XMLDataStorage()
        {
            //all works fine
            _storeState = true;
        }
    }
}
