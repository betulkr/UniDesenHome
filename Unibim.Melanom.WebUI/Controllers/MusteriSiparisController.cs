using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.Toastr;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class MusteriSiparisController : Controller
    {
        // GET: MusteriSiparis
        public ActionResult Tumu()
        {
            var model = DbModel.EF.Siparis.OrderByDescending(x => x.siparisId).ToList();

            return View(model);
        }

        public ActionResult Tamamlanan()
        {
            var model = DbModel.EF.Siparis.Where(x=>x.durum== "Tamamlandı.").OrderByDescending(x => x.siparisId).ToList();

            return View(model);
        }

        public ActionResult Bekleyen()
        {
            var model = DbModel.EF.Siparis.Where(x => x.durum == "Bayi siparişi onayladı.").OrderByDescending(x => x.siparisId).ToList();

            return View(model);
        }

        public ActionResult Tamamla(int id)
        {
            var model = DbModel.EF.Siparis.Where(x=>x.siparisId==id).FirstOrDefault();
            model.durum = "Tamamlandı.";
            DbModel.EF.SaveChanges();
            ToastrService.AddToUserQueue(new Toastr(message: "Sipariş tamamlandı.", title: "Başarılı", type: ToastrType.Success));
            return RedirectToAction("Bekleyen");
        }

        public ActionResult Detay(int id)
        {
            var model = DbModel.EF.Siparis.Where(x => x.siparisId == id).FirstOrDefault();

            return View(model);
        }
    }
}