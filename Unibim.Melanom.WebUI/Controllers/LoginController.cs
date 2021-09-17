using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Unibim.Melanom.WebUI.Models.EntityModel;
using Unibim.Melanom.WebUI.Models.Connection;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string sifre, int yetki)
        {
            sifre = FormsAuthentication.HashPasswordForStoringInConfigFile(sifre, "md5");
            string yetkiAd = "";

            var kullanici = DbModel.EF.Kullanici.AsNoTracking().Where(x => x.kullaniciAdi == email && x.sifre ==sifre ).FirstOrDefault();
            if (kullanici != null)
            {
                var kullaniciYetki = DbModel.EF.KullaniciYetki.Where(x => x.kullaniciId == kullanici.kullaniciId && x.yetkiId == yetki).FirstOrDefault();
                if (kullaniciYetki != null)
                {


                    YetkiliKullanici yetkiliKullanici = new YetkiliKullanici()
                    {
                        kullaniciId = kullanici.kullaniciId,
                        ad = kullanici.ad,
                        soyad = kullanici.soyad,
                        kullaniciAdi = kullanici.kullaniciAdi,
                        resim = kullanici.gorsel,
                        sifre = kullanici.sifre
                    };

                    if (yetki==1)
                    {
                        yetkiAd = "Yonetici";
                        Session["Yonetici"] = yetkiliKullanici;

                        
                    }
                    else if (yetki==2)
                    {
                        yetkiAd = "Bayi";
                        Session["Bayi"] = yetkiliKullanici;
                    }
                    else
                    {
                        yetkiAd = "Login";
                    }

                    FormsAuthentication.SetAuthCookie(email, true);

                    return RedirectToAction("Index", yetkiAd);
                }
                else
                {
                    TempData["mesaj"] = "YetkisizKullanici";
                    return View();
                }
            }
            else
            {
                TempData["mesaj"] = "KullaniciHatalı";
                return View();
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Login");
        }
    }
}