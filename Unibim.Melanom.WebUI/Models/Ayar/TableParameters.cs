using System;
using System.Collections.Generic;

namespace Unibim.Melanom.WebUI
{
    [Serializable]
    public class TableParameters<T> : FetchParameters
    {
        public List<T> Data { get; set; }

        public TableParameters()
        {
            Data = new List<T>();
        }
    }
}