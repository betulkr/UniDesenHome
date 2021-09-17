using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class TakimViewModel
    {
        public Takim takim { get; set; }
        public List<TakimGorsel> TakimGorsels { get; set; }

    }
}