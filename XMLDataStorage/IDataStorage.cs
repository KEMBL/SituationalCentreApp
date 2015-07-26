using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDataStorageLib
{
    /// <summary>
    /// This interface allows use different types of data storages
    /// </summary>
    public interface IDataStorage
    {
        bool StoreLoaded { get; }
    }
}
