using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class MusteriController : Controller
    {
        [HttpGet]
        public ActionResult Liste()
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            TableParameters<Musteri> param = new TableParameters<Musteri>();
            param.PageCount = 10;
            param.PageIndex = 0;
            param.SortExpression = "musteriId desc";
            param.SearchKey = null;

            var result = FetchMusteri(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression });
            if (result.IsSuccess)
            {
                param.TotalCount = result.TotalCount;
                param.Data = result.Data;
            }

            return View("~/Views/Musteri/Liste.cshtml", param);
        }

        [HttpPost]
        public ActionResult Liste(TableParameters<Musteri> param, string fc)
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            param.SortExpression = (string.IsNullOrEmpty(param.SortExpression) ? "musteriId desc" : param.SortExpression);
            param.PageCount = param.PageCount == 0 ? 10 : param.PageCount;

            var result = FetchMusteri(new FetchParameters() { PageCount = param.PageCount, PageIndex = param.PageIndex, SortExpression = param.SortExpression, SearchKey = param.SearchKey });
            param.TotalCount = result.TotalCount;
            param.Data = result.Data;

            return Json(new
            {
                Model = this.RenderPartialView("~/Views/Musteri/Partial/_Liste.cshtml", param.Data),
                Pager = Extensions.StrajaxPager(result.TotalCount, param.PageIndex, param.PageCount, "table-list")
            });
        }

        public Result<List<Musteri>> FetchMusteri(FetchParameters parameters)
        {
            int skip = parameters.PageIndex * parameters.PageCount;
            var veri = DbModel.EF.Musteri.OrderByDescending(x => x.musteriId).ToList();
            var listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            if (parameters.SearchKey != null)
            {
                string key = parameters.SearchKey.ToString().ToLower();
                veri = veri.Where(x =>
                                       x.adSoyad.ToString().ToLower().Contains(key) ||
                                       x.cep1.ToString().ToLower().Contains(key) ||
                                       x.cep2.ToString().ToLower().Contains(key) ||
                                       x.il.ToString().ToLower().Contains(key) ||
                                       x.ilce.ToString().ToLower().Contains(key) ||
                                       x.teslimAdresi.ToString().ToLower().Contains(key)
                                       ).ToList();
                listVeri = veri.Skip(skip).Take(parameters.PageCount).ToList();
            }

            if (veri != null && veri.Count > 0)
            {
                return new Result<List<Musteri>>(true, ResultType.Information, "Müşteriler listelendi.", listVeri, veri.Count());
            }
            else
            {
                return new Result<List<Musteri>>(false, ResultType.Warning, "Kayıt yok");
            }
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            var data = DbModel.EF.Musteri.Where(x => x.musteriId == id).FirstOrDefault();

            return View(data);

        }
    }
}