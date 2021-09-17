using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.Toastr;
using System.Data.Entity.Migrations;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class BayiIslemController : Controller
    {
        [HttpGet]
        public ActionResult Kayit()
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);
            return View(new Bayi());
        }

        [HttpPost]
        [Obsolete]
        public ActionResult Kayit(Bayi bayi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bayi.Kullanici.sifre= FormsAuthentication.HashPasswordForStoringInConfigFile("DesenHome_2021", "md5");
                    bayi.Kullanici.gorsel = "bos.png";

                    DbModel.EF.Bayi.Add(bayi);
                    DbModel.EF.SaveChanges();

                    KullaniciYetki ky = new KullaniciYetki()
                    {
                        kullaniciId = bayi.kullaniciId,
                        yetkiId = 2
                    };

                    DbModel.EF.KullaniciYetki.Add(ky);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Bayi başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                    return RedirectToAction("Liste", "BayiIslem");
                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Bayi eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                    return View(bayi);
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
                return View(bayi);
            }

          
        }

        [HttpGet]
        public ActionResult Liste()
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            TableParameters<Bayi> param = new TableParameters<Bayi>();
            param.PageCount = 10;
            param.PageIndex = 0;
            param.SortExpression = "bayiId desc";
            param.SearchKey = null;

            var result = FetchBayi(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression });
            if (result.IsSuccess)
            {
                param.TotalCount = result.TotalCount;
                param.Data = result.Data;
            }

            return View("~/Views/BayiIslem/Liste.cshtml", param);
        }

        [HttpPost]
        public ActionResult Liste(TableParameters<Bayi> param, string fc)
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            param.SortExpression = (string.IsNullOrEmpty(param.SortExpression) ? "bayiId desc" : param.SortExpression);
            param.PageCount = param.PageCount == 0 ? 10 : param.PageCount;

            var result = FetchBayi(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression, SearchKey = param.SearchKey });
            param.TotalCount = result.TotalCount;
            param.Data = result.Data;

            return Json(new
            {
                Model = this.RenderPartialView("~/Views/BayiIslem/Partial/_Liste.cshtml", param.Data),
                Pager = Extensions.StrajaxPager(result.TotalCount, param.PageIndex, param.PageCount, "table-list")
            });
        }

        public Result<List<Bayi>> FetchBayi(FetchParameters parameters)
        {
            int skip = parameters.PageIndex * parameters.PageCount;
            var veri = DbModel.EF.Bayi.OrderByDescending(x => x.bayiId).ToList();
            var listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            if (parameters.SearchKey != null)
            {
                string key = parameters.SearchKey.ToString().ToLower();
                veri = veri.Where(x =>
                                       x.Kullanici.ad.ToString().ToLower().Contains(key) ||
                                       x.Kullanici.soyad.ToString().ToLower().Contains(key) ||
                                       x.bayiAd.ToString().ToLower().Contains(key) ||
                                       x.il.ToString().ToLower().Contains(key) ||
                                       x.ilce.ToString().ToLower().Contains(key) ||
                                       x.adres.ToString().ToLower().Contains(key) ||
                                       x.telefon1.ToString().ToLower().Contains(key) ||
                                       x.telefon2.ToString().ToLower().Contains(key) ||
                                       x.fax.ToString().ToLower().Contains(key)).ToList();
                listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            }

            if (veri != null && veri.Count > 0)
            {
                return new Result<List<Bayi>>(true, ResultType.Information, "Bayilar listelendi.", listVeri, veri.Count());
            }
            else
            {
                return new Result<List<Bayi>>(false, ResultType.Warning, "Kayıt yok");
            }
        }

        [HttpPost]
        public JsonResult IlceGetir(string il)
        {
            var ilces = DbModel.EF.İlceler.Where(x => x.il == il).Select(y => new
            {
                ilce = y.ilce
            }).ToList();


            return Json(ilces, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            var data = DbModel.EF.Bayi.Where(x => x.bayiId == id).FirstOrDefault();
            Session["bayiId"] = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult Duzenle(Bayi bayi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int bayiId = (int)Session["bayiId"];

                    var data = DbModel.EF.Bayi.Where(x => x.bayiId == bayiId).FirstOrDefault();

                    data.Kullanici.ad = bayi.Kullanici.ad;
                    data.Kullanici.soyad = bayi.Kullanici.soyad;
                    data.Kullanici.kullaniciAdi = bayi.Kullanici.kullaniciAdi;
                    data.bayiAd = bayi.bayiAd;
                    data.il = bayi.il;
                    data.ilce = bayi.ilce;
                    data.adres = bayi.adres;
                    data.telefon1 = bayi.telefon1;
                    data.telefon2 = bayi.telefon2;
                    data.fax = bayi.fax;
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Bayi bilgileri başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));


                    return RedirectToAction("Liste", "BayiIslem");
                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Bayi bilgieri düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                    return View(bayi);
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
                return View(bayi);
            }


        }
    }
}