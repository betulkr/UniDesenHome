using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class SiparisOzet
    {
        public Musteri musteri { get; set; }
        public List<SepetUrun> sepetUruns { get; set; }
        public double toplamFiyat { get; set; }
        public double indirimliToplamFiyat { get; set; }
    }
}