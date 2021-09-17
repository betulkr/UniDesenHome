using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;
using Unibim.Melanom.WebUI.Models.EntityModel;
using Unibim.Melanom.WebUI.Models.Toastr;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class SiparisController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Il = new DropDownNesneleri().Il(null);

            if (Session["musteriId"] == null)
            {

                return View(new Musteri());
            }
            else
            {
                int musteriId = (int)Session["musteriId"];
                var musteri = DbModel.EF.Musteri.Where(x => x.musteriId == musteriId).FirstOrDefault();

                return View(musteri);
            }
        }

        [HttpPost]
        public ActionResult Index(Musteri m)
        {
            try
            {
                if (m.tc!=null && m.adSoyad != null && m.cep1 != null && m.cep2 != null && m.doğumTarihi != null && m.cinsiyet != null && m.medeniHal != null && m.cocuk != null && m.eğitimDurumu != null && m.meslek != null && m.il != null && m.ilce != null &&
                    m.semt != null && m.postaKodu != null && m.teslimAdresi != null && m.teslimAdresi != null && m.isAdres != null || m.eposta!=null )
                {
                    if (Session["musteriId"] == null)
                    {
                        DbModel.EF.Musteri.Add(m);
                        DbModel.EF.SaveChanges();

                        Session["musteriId"] = m.musteriId;
                    }
                    else
                    {
                        int musteriId = (int)Session["musteriId"];
                        var musteri = DbModel.EF.Musteri.Where(x => x.musteriId == musteriId).FirstOrDefault();

                        musteri.tc = m.tc;
                        musteri.adSoyad = m.adSoyad;
                        musteri.doğumTarihi = m.doğumTarihi;
                        musteri.cinsiyet = m.cinsiyet;
                        musteri.medeniHal = m.medeniHal;
                        musteri.cocuk = m.cocuk;
                        musteri.eğitimDurumu = m.eğitimDurumu;
                        musteri.meslek = m.meslek;
                        musteri.cep1 = m.cep1;
                        musteri.cep2 = m.cep2;
                        musteri.evtel = m.evtel;
                        musteri.istel = m.istel;
                        musteri.il = m.il;
                        musteri.ilce = m.ilce;
                        musteri.semt = m.semt;
                        musteri.postaKodu = m.postaKodu;
                        musteri.teslimAdresi = m.teslimAdresi;
                        musteri.isAdres = m.isAdres;
                        musteri.eposta = m.eposta;
                        DbModel.EF.SaveChanges();


                    }

                    ToastrService.AddToUserQueue(new Toastr(message: "Müşteri başarıyla kaydedilmiştir. Müşterinin istediği ürünleri seçebilirsiniz.", title: "Başarılı", type: ToastrType.Success));

                    return RedirectToAction("Urun");
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "Doldurulması zorunlu alanları doldurunuz.", title: "Hata", type: ToastrType.Error));
                    ViewBag.Il = new DropDownNesneleri().Il(null);

                    return View(m);
                }


            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Müşteri eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                ViewBag.Il = new DropDownNesneleri().Il(null);

                return View(m);
            }
        }

        [HttpGet]
        public ActionResult Urun()
        {
            if (Session["musteriId"] == null)
            {

                return RedirectToAction("Index");
            }
            else
            {
                var uruns = DbModel.EF.Urun.OrderBy(x => x.takimId).Where(x => x.adet > 0).ToList();
                return View(uruns);

            }
        }

        [HttpPost]
        public ActionResult Urun(int[] urunId, int[] adet)
        {
            try
            {
                if (urunId.Length > 0)
                {
                    List<int> urunAdet = new List<int>();

                    foreach (var i in adet)
                    {
                        if (i > 0)
                        {
                            urunAdet.Add(i);
                        }
                    }
                    if (urunAdet.Count > 0 && urunAdet != null && (urunAdet.Count == urunId.Length))
                    {
                        if (Session["sepetId"] != null)
                        {
                            int sepetId = (int)Session["sepetId"];
                            var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();
                            DbModel.EF.SepetUrun.RemoveRange(sepetUruns);
                            DbModel.EF.SaveChanges();

                            for (int i = 0; i < urunId.Length; i++)
                            {
                                for (int j = 0; i < urunAdet[i]; i++)
                                {
                                    SepetUrun su = new SepetUrun()
                                    {
                                        sepetId = sepetId,
                                        urunId = urunId[i],
                                        adet = urunAdet[i]
                                    };
                                    DbModel.EF.SepetUrun.Add(su);
                                }

                            }
                            DbModel.EF.SaveChanges();

                            Session["sepetId"] = sepetId;
                        }
                        else
                        {
                            Sepet s = new Sepet();
                            DbModel.EF.Sepet.Add(s);
                            DbModel.EF.SaveChanges();

                            for (int i = 0; i < urunId.Length; i++)
                            {
                                for (int j = 0; j < urunAdet[i]; j++)
                                {
                                    SepetUrun su = new SepetUrun()
                                    {
                                        sepetId = s.sepetId,
                                        urunId = urunId[i],
                                        adet = urunAdet[i]
                                    };
                                    DbModel.EF.SepetUrun.Add(su);
                                }
                            }
                            DbModel.EF.SaveChanges();

                            Session["sepetId"] = s.sepetId;
                        }


                        ToastrService.AddToUserQueue(new Toastr(message: "Ürünler  başarıyla seçilmiştir.", title: "Başarılı", type: ToastrType.Success));

                        return RedirectToAction("Sepet");
                    }
                    else
                    {
                        ToastrService.AddToUserQueue(new Toastr(message: "İşlem gerçekleştirmek için seçtiğiniz ürünlerin adedini belirtiniz. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                    }
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: "İşlem gerçekleştirmek için en az bir ürün seçmeniz gerekir. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                }

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "İşlem gerçekleştirilirken bir hata meydana geldi. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));

            }

            return RedirectToAction("Urun");
        }

        [HttpGet]
        public ActionResult Sepet()
        {
            if (Session["musteriId"] == null)
            {

                return RedirectToAction("Index");
            }
            else if (Session["sepetId"] == null)
            {
                return RedirectToAction("Urun");
            }
            else
            {
                int sepetId = (int)Session["sepetId"];
                var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();

                return View(sepetUruns);
            };
        }

        [HttpPost]
        public ActionResult Sepet(string[] sepetUrunOzelliks)
        {
            try
            {

                foreach (var s in sepetUrunOzelliks)
                {
                    string[] urunOzelliks = s.Split('-');
                    int sepetUrunId = Convert.ToInt32(urunOzelliks[0]);
                    int urunOzellikId = Convert.ToInt32(urunOzelliks[1]);
                    int sepetId = (int)Session["sepetId"];
                    //int ozellikTurId = (int) DbModel.EF.Ozellik.Where(x => x.ozellikId == urunOzellikId).FirstOrDefault().ozellikTurId;
                    var ozellikDahaOnceEklenmisMi = DbModel.EF.SepetUrunOzellik.Where(x => x.SepetUrun.sepetUrunId == sepetUrunId && x.urunOzellikId == urunOzellikId ).ToList();
                    if (ozellikDahaOnceEklenmisMi.Count==0)
                    {
                        SepetUrunOzellik sepetUrunOzellik = new SepetUrunOzellik()
                        {
                            sepetUrunId = sepetUrunId,
                            urunOzellikId = urunOzellikId
                        };
                        DbModel.EF.SepetUrunOzellik.Add(sepetUrunOzellik);
                        DbModel.EF.SaveChanges();
                    }
                    
                }

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün özellikleri başarıyla kaydedilmiştir.", title: "Başarılı", type: ToastrType.Success));

                Session["urunOzellik"] = "Ozellikler eklendi";

                return RedirectToAction("Indirim");

            }
            catch (Exception)
            {

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün özellikleri eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return RedirectToAction("Sepet");
        }

        [HttpGet]
        public ActionResult Indirim()
        {
            if (Session["musteriId"] == null)
            {

                return RedirectToAction("Index");
            }
            else if (Session["sepetId"] == null)
            {
                return RedirectToAction("Urun");
            }
            else if (Session["urunOzellik"] == null)
            {
                return RedirectToAction("Sepet");
            }
            else
            {
                int sepetId = (int)Session["sepetId"];
                var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();

                return View(sepetUruns);
            };
        }

        [HttpPost]
        public ActionResult Indirim(int[] indirimOrans)
        {
            try
            {
                int sepetId = (int)Session["sepetId"];
                var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();
                double toplamFiyat = 0;

                for (int i = 0; i < indirimOrans.Length; i++)
                {
                    double indirimliFiyat = (double)(sepetUruns[i].Urun.fiyat - ((sepetUruns[i].Urun.fiyat * indirimOrans[i]) / 100));
                    sepetUruns[i].indirimliFiyat = indirimliFiyat;
                    sepetUruns[i].indirimOranı = indirimOrans[i];
                    toplamFiyat = toplamFiyat + indirimliFiyat;
                }

                Session["toplamFiyat"] = toplamFiyat;
                Session["indirim"] = "İndirim uygulandı.";

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün indirimleri başarıyla kaydedilmiştir.", title: "Başarılı", type: ToastrType.Success));

                return RedirectToAction("Dogrulama");
            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Ürün indirimleri eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
                return RedirectToAction("Indirim");
            }
        }

        [HttpGet]
        public ActionResult Dogrulama()
        {
            if (Session["musteriId"] == null)
            {

                return RedirectToAction("Index");
            }
            else if (Session["sepetId"] == null)
            {
                return RedirectToAction("Urun");
            }
            else if (Session["urunOzellik"] == null)
            {
                return RedirectToAction("Sepet");
            }
            else if (Session["indirim"] == null)
            {
                return RedirectToAction("Indirim");
            }
            else
            {
                int sepetId = (int)Session["sepetId"];
                int musteriId = (int)Session["musteriId"];
                var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();
                double toplamFiyat = (double)sepetUruns.Sum(x => x.Urun.fiyat);
                double indirimliToplamFiyat = (double)Session["toplamFiyat"];

                SiparisOzet model = new SiparisOzet()
                {
                    sepetUruns = sepetUruns,
                    musteri = DbModel.EF.Musteri.Where(x => x.musteriId == musteriId).FirstOrDefault(),
                    indirimliToplamFiyat = indirimliToplamFiyat,
                    toplamFiyat = toplamFiyat
                };


                return View(model);
            };
        }

        [HttpPost]
        public ActionResult Dogrulama(Siparis s)
        {
            try
            {
                YetkiliKullanici yetkili = (Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici)Session["Bayi"];
                int sepetId = (int)Session["sepetId"];
                var bayi=DbModel.EF.Bayi.Where(x => x.kullaniciId == yetkili.kullaniciId).FirstOrDefault();
                s.bayiId = bayi.bayiId;
                s.sepetId = sepetId;
                s.musteriId = (int)Session["musteriId"];
                s.siparisNo = Guid.NewGuid().ToString();
                s.siparisTarihi = DateTime.Now;
                s.onaylandiMi = true;
                s.durum = "Bayi siparişi onayladı.";
                s.tutar = (double)Session["toplamFiyat"];
                DbModel.EF.Siparis.Add(s);
                DbModel.EF.SaveChanges();

                var sepetUruns = DbModel.EF.SepetUrun.Where(x => x.sepetId == sepetId).ToList();

                foreach (var su in sepetUruns)
                {
                    var urun = DbModel.EF.Urun.Where(x => x.urunId == su.urunId).FirstOrDefault();
                    if (urun.adet > 0)
                    {
                        urun.adet = urun.adet - 1;
                        DbModel.EF.SaveChanges();
                    }
                }

                ToastrService.AddToUserQueue(new Toastr(message: "Sipariş başarıyla onaylanmıştır.", title: "Başarılı", type: ToastrType.Success));

                return RedirectToAction("Liste");
            }
            catch
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Sipariş onaylanırken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));

                return RedirectToAction("Dogrulama");
            }


        }

        [HttpGet]
        public ActionResult Liste()
        {

            var model = DbModel.EF.Siparis.OrderByDescending(x => x.siparisId).ToList();

            return View(model);

        }

        [HttpGet]
        public ActionResult Detay(int id)
        {

            var model = DbModel.EF.Siparis.OrderByDescending(x => x.siparisId==id).FirstOrDefault();

            return View(model);

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
    }
}