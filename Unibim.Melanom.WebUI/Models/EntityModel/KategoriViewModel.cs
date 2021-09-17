using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class KategoriViewModel
    {
        public Kategori kategori { get; set; }
        public List<Altkategori> altkategoris { get; set; }
        //public List<Takim> takims { get; set; }
        public List<Urun> uruns { get; set; }
    }
}