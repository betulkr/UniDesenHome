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
    public class AltKategoriController : Controller
    {

        [HttpPost]
        public ActionResult Kayit(Altkategori k)
        {
            if (k.kategoriId!=null && k.ad !=null && k.icon!=null)
            {
                try
                {

                    DbModel.EF.Altkategori.Add(k);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori başarıyla eklendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori eklenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "AltKategori");
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            
            var data = DbModel.EF.Altkategori.Where(x => x.altkategoriId == id).FirstOrDefault();

            ViewBag.Kategori = new DropDownNesneleri().Kategori(null);

            Session["altkategoriId"] = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult Duzenle(Altkategori k)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = (int)Session["altkategoriId"];
                    var data = DbModel.EF.Altkategori.Where(x => x.altkategoriId == id).FirstOrDefault();
                    data.kategoriId = k.kategoriId;
                    data.ad = k.ad;

                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori başarıyla düzenlendi.", title: "Başarılı", type: ToastrType.Success));

                }
                catch (Exception ex)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori düzenlenirken bir hata meydana geldi. Lütfen tekrar deneyin.", title: "Hata", type: ToastrType.Error));
                }
            }
            else
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Tüm alanları doldurduğunuzdan emin olun.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "AltKategori");
        }

        [HttpGet]
        public ActionResult Sil(int id)
        {
            try
            {
                var data = DbModel.EF.Altkategori.Where(x => x.altkategoriId == id).FirstOrDefault();
                var takim = DbModel.EF.Takim.Where(x => x.altkategoriId == id).ToList();

                if (takim.Count > 0)
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Silmek istediğiniz alt kategoriye ait Takım ve ürünler bulunmaktadır.", title: "Uyarı", type: ToastrType.Warning));
                }
                else
                {
                    DbModel.EF.Altkategori.Remove(data);
                    DbModel.EF.SaveChanges();

                    ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori başarıyla silindi.", title: "Başarılı", type: ToastrType.Success));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Alt kategori silinirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Liste", "AltKategori");
        }

        [HttpGet]
        public ActionResult Liste()
        {

            TableParameters<Altkategori> param = new TableParameters<Altkategori>();
            param.PageCount = 10;
            param.PageIndex = 0;
            param.SortExpression = "altkategoriId desc";
            param.SearchKey = null;

            var result = FetchKategori(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression });
            if (result.IsSuccess)
            {
                param.TotalCount = result.TotalCount;
                param.Data = result.Data;
            }

            return View("~/Views/AltKategori/Liste.cshtml", param);
        }

        [HttpPost]
        public ActionResult Liste(TableParameters<Altkategori> param, string fc)
        {


            param.SortExpression = (string.IsNullOrEmpty(param.SortExpression) ? "altkategoriId desc" : param.SortExpression);
            param.PageCount = param.PageCount == 0 ? 10 : param.PageCount;

            var result = FetchKategori(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression, SearchKey = param.SearchKey });
            param.TotalCount = result.TotalCount;
            param.Data = result.Data;

            return Json(new
            {
                Model = this.RenderPartialView("~/Views/AltKategori/Partial/_Liste.cshtml", param.Data),
                Pager = Extensions.StrajaxPager(result.TotalCount, param.PageIndex, param.PageCount, "table-list")
            });
        }

        public Result<List<Altkategori>> FetchKategori(FetchParameters parameters)
        {
            ViewBag.Kategori = new DropDownNesneleri().Kategori(null);

            int skip = parameters.PageIndex * parameters.PageCount;
            var veri = DbModel.EF.Altkategori.OrderByDescending(x => x.altkategoriId).ToList();
            var listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            if (parameters.SearchKey != null)
            {
                string key = parameters.SearchKey.ToString().ToLower();
                veri = veri.Where(x =>
                                       x.ad.ToString().ToLower().Contains(key)
                                       ||
                                       x.Kategori.ad.ToString().ToLower().Contains(key)
                                       ).ToList();
                listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            }

            if (veri != null && veri.Count > 0)
            {
                return new Result<List<Altkategori>>(true, ResultType.Information, "Alt Kategoriler listelendi.", listVeri, veri.Count());
            }
            else
            {
                return new Result<List<Altkategori>>(false, ResultType.Warning, "Kayıt yok");
            }
        }
    }
}