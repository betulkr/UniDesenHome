using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class UrunViewModel
    {
        public Urun urun { get; set; }
        public List<UrunGorsel> urunGorsels { get; set; }
        public Takim takim { get; set; }
        public List<TakimGorsel> takimGorsels { get; set; }
    }
}