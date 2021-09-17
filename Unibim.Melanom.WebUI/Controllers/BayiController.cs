using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.EntityModel;
using Unibim.Melanom.WebUI.Models.Toastr;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class BayiController : Controller
    {
        // GET: Bayi
        public ActionResult Index()
        {
            
            YetkiliKullanici yetkili = (Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici)Session["Bayi"];
            int  bayiId = DbModel.EF.Bayi.Where(x => x.kullaniciId == yetkili.kullaniciId).FirstOrDefault().bayiId;

            var siparis = DbModel.EF.Siparis.Where(x=>x.bayiId==bayiId).OrderByDescending(x => x.siparisId).ToList();

            YoneticiViewModel model = new YoneticiViewModel()
            {
                tamamlanan = siparis.Where(x => x.durum == "Tamamlandı.").OrderByDescending(x => x.siparisId).ToList().Count(),
                bekleyen = siparis.Where(x => x.durum == "Bayi siparişi onayladı.").OrderByDescending(x => x.siparisId).ToList().Count(),
                tumu = siparis.Count()
            };


            return View(model);
        }
    }
}