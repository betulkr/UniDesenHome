using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Melanom.WebUI.Models.EntityModel;
using Unibim.Melanom.WebUI.Models.Connection;

namespace Unibim.Melanom.WebUI.Controllers
{
    //[RoutePrefix("Desenhome")]
    //[Route("{action=Index}")]
    public class KurumsalController : Controller
    {
        //[Route]
        [Route("~/")]
        [Route("~/Anasayfa")]
        public ActionResult Index()
        {

            GetSiteValue();
            return View();

        }

        [Route("~/Takimlar/{id}")]
        public ActionResult Takim(string id)
        {
            GetSiteValue();
            int altkategoriId = GetIdValue(id);
            List<TakimViewModel> models = new List<TakimViewModel>();

            Session["ad"] = DbModel.EF.Altkategori.Where(x => x.altkategoriId == altkategoriId).FirstOrDefault().ad;
            var takims = DbModel.EF.Takim.Where(x => x.altkategoriId == altkategoriId).OrderByDescending(x=>x.takimId).ToList();
            foreach (var t in takims)
            {
                TakimViewModel model = new TakimViewModel()
                {
                    takim=t,
                    TakimGorsels=DbModel.EF.TakimGorsel.Where(x=>x.takimId==t.takimId).ToList()
                };
                models.Add(model);
            }
            
            return View(models);
        }

        [Route("~/Urunler/{id}")]
        public ActionResult Urun(string id)
        {
           
            int takimId = GetIdValue(id);

            List<UrunViewModel> models = new List<UrunViewModel>();
            var uruns = DbModel.EF.Urun.Where(x => x.takimId == takimId).OrderByDescending(x => x.urunId).ToList();

            foreach (var u in uruns)
            {
                UrunViewModel model = new UrunViewModel()
                {
                    takim= DbModel.EF.Takim.Where(x => x.takimId == takimId).FirstOrDefault(),
                    takimGorsels = DbModel.EF.TakimGorsel.Where(x => x.takimId == takimId).ToList(),
                    urun = u,
                    urunGorsels=DbModel.EF.UrunGorsel.Where(x=>x.urunId==u.urunId).ToList()
                };
                models.Add(model);
            }

            GetSiteValue();

            return View(models);
        }

        [Route("~/UrunDetay/{id}")]
        public ActionResult UrunDetay(string id)
        {
            GetSiteValue();
            
            return View();
        }


        public void GetSiteValue()
        {

            var kategoris = DbModel.EF.Kategori.ToList();
            List<KategoriViewModel> models = new List<KategoriViewModel>();

            foreach (var k in kategoris)
            {
                KategoriViewModel model = new KategoriViewModel()
                {
                    kategori = k,
                    altkategoris = DbModel.EF.Altkategori.Where(x => x.kategoriId == k.kategoriId).ToList(),
                    uruns= DbModel.EF.Urun.Where(x => x.Takim.Altkategori.kategoriId == k.kategoriId).ToList()
                };
                models.Add(model);
            }
            Session["model"] = models;
        }

        private int GetIdValue(string url)
        {
            string[] key = url.Split('-');
            int id = Convert.ToInt32(key[0]);

            return id;
        }
    }
}