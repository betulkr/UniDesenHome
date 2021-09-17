using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.Toastr;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class TakimController : Controller
    {
        [HttpGet]
        public ActionResult Kayit()
        {
            ViewBag.Kategori = new DropDownNesneleri().Altkategori(null);
            return View(new Takim());
        }

        [HttpPost]
        public ActionResult Kayit(Takim t,  HttpPostedFileBase resim)
        {

            try
            {
                if (t.altkategoriId != null && t.takimAdi != null && t.aciklamaBaslik != null && t.aciklama != null && resim != null)
                {
                  
                    string resimAd = ResimEkle(resim, "~/Content/Images/Takım/", 360, 300);

                    if (resimAd == "Resim eklenirken bir hata meydana geldi.")
                    {
                        ModelState.AddModelError("resim", resimAd);
                    }
                    if (resimAd == "Lütfen png,jpg,jpeg uzantılı resimler ekleyiniz.")
                    {
                        ModelState.AddModelError("resim", resimAd);
                    }
                    if (resimAd == "Maksimum resim boyutunu aştınız.")
                    {
                        ModelState.AddModelError("resim", resimAd);
                    }

                    if (ModelState.IsValid)
                    {
                        t.aciklamaGorsel = resimAd;
                        DbModel.EF.Takim.Add(t);
                        DbModel.EF.SaveChanges();
                        

                        ToastrService.AddToUserQueue(new Toastr(message: "Takım başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                        return RedirectToAction("Liste", "Takim");
                    }
                    else
                    {
                        ToastrService.AddToUserQueue(new Toastr(message: "Takım eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));

                    }
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Lütfen tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
                }
            }
            catch (Exception ex)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Takım eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));

            }

            ViewBag.Kategori = new DropDownNesneleri().Altkategori(null);
            return View(t);
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {

            var data = DbModel.EF.Takim.Where(x => x.takimId == id).FirstOrDefault();

            ViewBag.Kategori = new DropDownNesneleri().Altkategori(null);

            Session["takimId"] = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult Duzenle(Takim t, HttpPostedFileBase slider, HttpPostedFileBase resim)
        {

            try
            {
                int id = (int)Session["takimId"];
                t.takimId = id;
                var data = DbModel.EF.Takim.Where(x => x.takimId == id).FirstOrDefault();

                if (t.altkategoriId != null && t.takimAdi != null && t.aciklamaBaslik != null && t.aciklama != null)
                {
                    if (slider != null)
                    {
                        var sliderGorsel = DbModel.EF.TakimGorsel.Where(x => x.takimId == id).FirstOrDefault();
                        bool silindiMi = ResimSil(sliderGorsel.gorsel, "~/Content/Images/Takım/");
                        if (silindiMi)
                        {
                            string sliderAd = ResimEkle(slider, "~/Content/Images/Takım/", 1350, 700);
                            if (sliderAd == "Resim eklenirken bir hata meydana geldi.")
                            {
                                ModelState.AddModelError("slider", sliderAd);
                            }
                            else if (sliderAd == "Lütfen png,jpg,jpeg uzantılı resimler ekleyiniz.")
                            {
                                ModelState.AddModelError("slider", sliderAd);
                            }
                            else if (sliderAd == "Maksimum resim boyutunu aştınız.")
                            {
                                ModelState.AddModelError("slider", sliderAd);
                            }
                            else
                            {
                                sliderGorsel.gorsel = sliderAd;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("slider", "Hata");
                        }

                    }
                    if (resim != null)
                    {
                        bool silindiMi = ResimSil(data.aciklamaGorsel, "~/Content/Images/Takım/");
                        if (silindiMi)
                        {
                            string resimAd = ResimEkle(resim, "~/Content/Images/Takım/", 360, 300);

                            if (resimAd == "Resim eklenirken bir hata meydana geldi.")
                            {
                                ModelState.AddModelError("resim", resimAd);
                            }
                            else if (resimAd == "Lütfen png,jpg,jpeg uzantılı resimler ekleyiniz.")
                            {
                                ModelState.AddModelError("resim", resimAd);
                            }
                            else if (resimAd == "Maksimum resim boyutunu aştınız.")
                            {
                                ModelState.AddModelError("resim", resimAd);
                            }
                            else
                            {
                                data.aciklamaGorsel = resimAd;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("resim", "Hata");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        data.altkategoriId = t.altkategoriId;
                        data.takimAdi = t.takimAdi;
                        data.aciklamaBaslik = t.aciklamaBaslik;
                        data.aciklama = t.aciklama;

                        DbModel.EF.SaveChanges();

                        ToastrService.AddToUserQueue(new Toastr(message: "Takım başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));

                        return RedirectToAction("Liste", "Takim");
                    }
                    else
                    {
                        ToastrService.AddToUserQueue(new Toastr(message: "Takım düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));

                    }
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Lütfen tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
                }

            }
            catch (Exception ex)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Takim düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
            }

            ViewBag.Kategori = new DropDownNesneleri().Altkategori(null);
            return View(t);
        }

        [HttpGet]
        public ActionResult Sil(int id)
        {
            try
            {
                var data = DbModel.EF.Takim.Where(x => x.takimId == id).FirstOrDefault();
                var urun = DbModel.EF.Urun.Where(x => x.takimId == id).ToList();

                if (urun.Count > 0)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Silmek istediğiniz takıma ait ürünler bulunmaktadır.", title: "Uyarı", type: ToastrType.Warning));
                }
                else
                {
                    var gorsel = DbModel.EF.TakimGorsel.Where(x => x.takimId == id).ToList();
                    if (gorsel.Count > 0)
                    {
                        DbModel.EF.TakimGorsel.RemoveRange(gorsel);
                        DbModel.EF.SaveChanges();
                    }

                    DbModel.EF.Takim.Remove(data);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Takım başarıyla silindi.", title: "Başarılı", type: ToastrType.Success));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Takım silinirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "Takim");
        }

        [HttpGet]
        public ActionResult Liste()
        {

            var data = DbModel.EF.Takim.ToList();

            return View(data);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            
            var data = DbModel.EF.Takim.Where(x=>x.altkategoriId==id).ToList();

            return View("~/Views/Takim/Liste.cshtml", data);
        }

        public ActionResult Gorsel(int id)
        {


            var takimGorsels = DbModel.EF.TakimGorsel.Where(x => x.takimId == id).ToList();
            Session["takimAd"] = DbModel.EF.Takim.Where(x => x.takimId == id).FirstOrDefault().takimAdi;
            Session["takimId"] = id;
            return View(takimGorsels);
        }

        [HttpPost]
        public ActionResult Gorsel(HttpPostedFileBase file)
        {
            int takimId = (int)Session["takimId"];

            try
            {
                string fileName = ResimEkle(file, "~/Content/Images/Takım/", 1350, 700);

                if (fileName == "Resim eklenirken bir hata meydana geldi.")
                {
                    ModelState.AddModelError("file", fileName);
                }
                if (fileName == "Lütfen png,jpg,jpeg uzantılı resimler ekleyiniz.")
                {
                    ModelState.AddModelError("file", fileName);
                }
                if (fileName == "Maksimum resim boyutunu aştınız.")
                {
                    ModelState.AddModelError("file", fileName);
                }

                if (ModelState.IsValid)
                {
                    TakimGorsel takimGorsel = new TakimGorsel()
                    {
                        takimId = takimId,
                        gorsel = fileName
                    };
                    DbModel.EF.TakimGorsel.Add(takimGorsel);
                    DbModel.EF.SaveChanges();


                    ToastrService.AddToUserQueue(new Toastr(message: "Takım görseli başarıyla eklendi. Ürüne ait başka görseller ekleyebilirsiniz.", title: "Başarılı", type: ToastrType.Success));
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: fileName, title: "Hata", type: ToastrType.Error));
                }
            }
            catch (Exception)
            {

                ToastrService.AddToUserQueue(new Toastr(message: "Takım görseli eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return Redirect("/Takim/Gorsel/" + takimId);
        }

        public ActionResult GorselSil(int id)
        {
            var takimGorsel = DbModel.EF.TakimGorsel.Where(x => x.takimGorselId == id).FirstOrDefault();
            int takimId = (int)takimGorsel.takimId;
            try
            {


                ResimSil(takimGorsel.gorsel, "~/Content/Images/Takim/");
                DbModel.EF.TakimGorsel.Remove(takimGorsel);
                DbModel.EF.SaveChanges();

                ToastrService.AddToUserQueue(new Toastr(message: "Takım görseli başarıyla silindi. Ürüne ait başka görseller ekleyebilirsiniz.", title: "Başarılı", type: ToastrType.Success));

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Takım görseli silinirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return Redirect("/Takim/Gorsel/" + takimId);
        }

        public string ResimEkle(HttpPostedFileBase resim, string yol, int w, int h)
        {
            string resimAd = "Resim eklenirken bir hata meydana geldi.";
            try
            {
                string uzanti = Path.GetExtension(resim.FileName).ToLower();
                if (uzanti == ".png" || uzanti == ".jpg" || uzanti == ".jpeg")
                {

                    if (resim.ContentLength > 1000000)
                    {
                        resimAd = "Maksimum resim boyutunu aştınız.";
                    }
                    else
                    {
                        Image orjinalResim = Image.FromStream(resim.InputStream);
                        resimAd = Guid.NewGuid() + uzanti;
                        Bitmap res = new Bitmap(orjinalResim, w, h);
                        string path = Path.Combine(Server.MapPath(yol), resimAd);
                        res.Save(path);
                    }
                }
                else
                    resimAd = "Lütfen png,jpg,jpeg uzantılı resimler ekleyiniz.";

            }
            catch (Exception)
            {
                resimAd = "Resim eklenirken bir hata meydana geldi.";
            }

            return resimAd;
        }

        public bool ResimSil(string resim, string yol)
        {
            try
            {
                if (resim != "user.png")
                {
                    string path = Path.Combine(Server.MapPath(yol), resim);
                    System.IO.File.Delete(path);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}