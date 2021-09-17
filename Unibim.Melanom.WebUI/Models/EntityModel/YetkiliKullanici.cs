using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class YetkiliKullanici
    {
        public int kullaniciId { get; set; }

        public string ad { get; set; }

        public string soyad { get; set; }

        public string kullaniciAdi { get; set; }

        public string resim { get; set; }

        public string sifre { get; set; }
    }
}