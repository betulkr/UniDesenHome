using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.Toastr;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class OzellikTurController : Controller
    {
        [HttpGet]
        public ActionResult Kayit()
        {

            return View(new OzellikTur());
        }

        [HttpPost]
        public ActionResult Kayit(OzellikTur o)
        {
            if (o.ad != null)
            {
                try
                {

                    DbModel.EF.OzellikTur.Add(o);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "OzellikTur");
        }

        [HttpPost]
        public ActionResult OzellikKayit(Ozellik o)
        {
            if (o.ad != null && o.ozellikTurId != null && o.kod != null)
            {
                try
                {
                    DbModel.EF.Ozellik.Add(o);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "OzellikTur");
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var data = DbModel.EF.OzellikTur.Where(x => x.ozellikTurId == id).FirstOrDefault();
            Session["ozellikTurId"] = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult Duzenle(OzellikTur o)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = (int)Session["ozellikTurId"];
                    var data = DbModel.EF.OzellikTur.Where(x => x.ozellikTurId == id).FirstOrDefault();
                    data.ad = o.ad;

                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "OzellikTur");
        }

        [HttpGet]
        public ActionResult Sil(int id)
        {
            try
            {
                var data = DbModel.EF.OzellikTur.Where(x => x.ozellikTurId == id).FirstOrDefault();
                var ozellik = DbModel.EF.Ozellik.Where(x => x.ozellikTurId == id).ToList();

                if (ozellik.Count > 0)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Silmek istediğiniz özellik türüne ait özellikler bulunmaktadır.", title: "Uyarı", type: ToastrType.Warning));
                }
                else
                {
                    DbModel.EF.OzellikTur.Remove(data);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü başarıyla silindi.", title: "Başarılı", type: ToastrType.Success));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Özellik türü silinirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                throw;
            }

            return RedirectToAction("Liste", "OzellikTur");
        }

        [HttpGet]
        public ActionResult Liste()
        {

            var veri = DbModel.EF.OzellikTur.OrderByDescending(x => x.ozellikTurId).ToList();

            return View(veri);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            Session["ozellikId"] = id;
            var veri = DbModel.EF.Ozellik.Where(x => x.ozellikTurId == id).OrderByDescending(x => x.ozellikId).ToList(); 

            return View(veri);
        }

        [HttpGet]
        public ActionResult OzellikDuzenle(int id)
        {
            var data = DbModel.EF.Ozellik.Where(x => x.ozellikId == id).FirstOrDefault();
            Session["ozellikId"] = id;

            ViewBag.OzellikTur = new DropDownNesneleri().OzellikTur(null);

            return View(data);
        }

        [HttpPost]
        public ActionResult OzellikDuzenle(Ozellik o)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int id = (int)Session["ozellikId"];
                    var data = DbModel.EF.Ozellik.Where(x => x.ozellikId == id).FirstOrDefault();
                    data.ad = o.ad;
                    data.kod = o.kod;
                    data.ozellikTurId = o.ozellikTurId;
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik  başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ViewBag.OzellikTur = new DropDownNesneleri().OzellikTur(null);
                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik  düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ViewBag.OzellikTur = new DropDownNesneleri().OzellikTur(null);
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return Redirect("/OzellikTur/Detay/"+o.ozellikTurId);
        }

        [HttpGet]
        public ActionResult OzellikSil(int id)
        {
            try
            {
                var data = DbModel.EF.Ozellik.Where(x => x.ozellikId == id).FirstOrDefault();
                var urun = DbModel.EF.UrunOzellik.Where(x => x.ozellikId == id).ToList();

                if (urun.Count > 0)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Silmek istediğiniz özellik türüne ait özellikler bulunmaktadır.", title: "Uyarı", type: ToastrType.Warning));
                }
                else
                {
                    DbModel.EF.Ozellik.Remove(data);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Özellik  başarıyla silindi.", title: "Başarılı", type: ToastrType.Success));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Özellik  silinirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                throw;
            }

            return RedirectToAction("Liste", "OzellikTur");
        }
    }
}