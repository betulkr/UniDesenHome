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
    public class KategoriController : Controller
    {
        [HttpGet]
        public ActionResult Kayit()
        {

            return View(new Kategori());
        }

        [HttpPost]
        public ActionResult Kayit(Kategori k)
        {
            if (k.ad != null)
            {
                try
                {

                    DbModel.EF.Kategori.Add(k);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Kategori başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Kategori eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "Kategori");
        }

        [HttpPost]
        public ActionResult AltkategoriKayit(Altkategori k)
        {
            if (k.ad != null && k.kategoriId != null)
            {
                try
                {


                    DbModel.EF.Altkategori.Add(k);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Alt Kategori başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));
                    return Redirect("/Kategori/Detay/" + k.kategoriId);
                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Alt Kategori eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                    return RedirectToAction("Liste", "Kategori");
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
                return RedirectToAction("Liste", "Kategori");
            }


        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var data = DbModel.EF.Kategori.Where(x => x.kategoriId == id).FirstOrDefault();
            Session["kategoriId"] = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult Duzenle(Kategori k)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = (int)Session["kategoriId"];
                    var data = DbModel.EF.Kategori.Where(x => x.kategoriId == id).FirstOrDefault();
                    data.ad = k.ad;

                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Kategori başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Kategori düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "Kategori");
        }

        [HttpGet]
        public ActionResult Sil(int id)
        {
            try
            {
                var data = DbModel.EF.Kategori.Where(x => x.kategoriId == id).FirstOrDefault();
                var altkategori = DbModel.EF.Altkategori.Where(x => x.kategoriId == id).ToList();

                if (altkategori.Count > 0)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Silmek istediğiniz kategoriye ait alt kategori bulunmaktadır.", title: "Uyarı", type: ToastrType.Warning));
                }
                else
                {
                    DbModel.EF.Kategori.Remove(data);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Kategori başarıyla silindi.", title: "Başarılı", type: ToastrType.Success));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Kategori silinirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                throw;
            }

            return RedirectToAction("Liste", "Kategori");
        }

        [HttpGet]
        public ActionResult Liste()
        {

            var model = DbModel.EF.Kategori.OrderByDescending(x => x.kategoriId).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {

            var model = DbModel.EF.Altkategori.Where(x => x.kategoriId == id).ToList();
            Session["kategoriId"] = id;
            return View(model);
        }

        public ActionResult Liste1()
        {
            return View();
        }
    }
}