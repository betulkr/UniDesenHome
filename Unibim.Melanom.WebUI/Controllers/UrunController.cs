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
    public class UrunController : Controller
    {
        // GET: Urun
        public ActionResult Kayit()
        {
            ViewBag.Takim = new DropDownNesneleri().Takim(null);
            ViewBag.Grup = new DropDownNesneleri().Grup(null);
            ViewBag.Ozellik = new DropDownNesneleri().Ozellik(null);

            return View(new Urun());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kayit(Urun urun,  int[] oz)
        {

            try
            {
                DbModel.EF.Urun.Add(urun);
                DbModel.EF.SaveChanges();

                if (oz.Length > 0)
                {
                    foreach (var i in oz)
                    {
                        UrunOzellik urunOzellik = new UrunOzellik()
                        {
                            urunId = urun.urunId,
                            ozellikId = i
                        };
                        DbModel.EF.UrunOzellik.Add(urunOzellik);
                        DbModel.EF.SaveChanges();
                    }
                }
                ToastrService.AddToUserQueue(new Toastr(message: "Ürün başarıyla eklendi. Ürüne ait görselleri ekleyebilirsiniz.", title: "Başarılı", type: ToastrType.Success));

                return RedirectToAction("Liste", "Urun");
            }
            catch (Exception)
            {
                ViewBag.Takim = new DropDownNesneleri().Takim(null);
                ViewBag.Grup = new DropDownNesneleri().Grup(null);
                ViewBag.Ozellik = new DropDownNesneleri().Ozellik(null);

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));

                return View(urun);
            }

        }


        public ActionResult Gorsel(int id)
        {
            

            var urunGorsels = DbModel.EF.UrunGorsel.Where(x => x.urunId == id).ToList();
            Session["urunAd"] = DbModel.EF.Urun.Where(x => x.urunId == id).FirstOrDefault().ad;
            Session["urunId"] = id;
            return View(urunGorsels);
        }

        [HttpPost]
        public ActionResult Gorsel(HttpPostedFileBase file)
        {
            int urunId = (int)Session["urunId"];
            
            try
            {
                string fileName = ResimEkle(file, "~/Content/Images/Urun/");

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
                    UrunGorsel urunGorsel = new UrunGorsel()
                    {
                        urunId = urunId,
                        gorsel = fileName
                    };
                    DbModel.EF.UrunGorsel.Add(urunGorsel);
                    DbModel.EF.SaveChanges();


                    ToastrService.AddToUserQueue(new Toastr(message: "Ürün görseli başarıyla eklendi. Ürüne ait başka görseller ekleyebilirsiniz.", title: "Başarılı", type: ToastrType.Success));
                }
                else
                {
                    ToastrService.AddToUserQueue(new Toastr(message: fileName, title: "Hata", type: ToastrType.Error));
                }
            }
            catch (Exception)
            {

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün görseli eklenirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return Redirect("/Urun/Gorsel/" + urunId);
        }

        public ActionResult GorselSil(int id)
        {
            var urunGorsel = DbModel.EF.UrunGorsel.Where(x => x.urunGorselId == id).FirstOrDefault();
            int urunId = (int) urunGorsel.urunId;
            try
            {
                

                ResimSil(urunGorsel.gorsel, "~/Content/Images/Urun/");
                DbModel.EF.UrunGorsel.Remove(urunGorsel);
                DbModel.EF.SaveChanges();

                ToastrService.AddToUserQueue(new Toastr(message: "Ürün görseli başarıyla silindi. Ürüne ait başka görseller ekleyebilirsiniz.", title: "Başarılı", type: ToastrType.Success));

            }
            catch (Exception)
            {
                ToastrService.AddToUserQueue(new Toastr(message: "Ürün görseli silinirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.", title: "Hata", type: ToastrType.Error));
            }

            return Redirect("/Urun/Gorsel/" + urunId);
        }

        public ActionResult Liste()
        {
            var uruns = DbModel.EF.Urun.OrderByDescending(x => x.urunId).ToList();

            return View(uruns);
        }


        public string ResimEkle(HttpPostedFileBase resim, string yol)
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
                        Bitmap res = new Bitmap(orjinalResim, 640, 480);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(HttpPostedFileBase aUploadedFile)
        {
            var vReturnImagePath = string.Empty;
            if (aUploadedFile.ContentLength > 0)
            {
                var vFileName = Path.GetFileNameWithoutExtension(aUploadedFile.FileName);
                var vExtension = Path.GetExtension(aUploadedFile.FileName);

                string sImageName = vFileName + DateTime.Now.ToString("YYYYMMDDHHMMSS");

                var vImageSavePath = Server.MapPath("~/Content/UpImages/") + sImageName + vExtension;
                //sImageName = sImageName + vExtension;  
                //string server = Request.Url.Authority;
                vReturnImagePath = "https://prosbati.com/Content/UpImages/" + sImageName + vExtension;
                ViewBag.Msg = vImageSavePath;
                var path = vImageSavePath;

                // Saving Image in Original Mode  
                aUploadedFile.SaveAs(path);
                var vImageLength = new FileInfo(path).Length;
                //here to add Image Path to You Database ,  
                TempData["message"] = string.Format("Image was Added Successfully");
            }
            return Json(Convert.ToString(vReturnImagePath), JsonRequestBehavior.AllowGet);
        }
    }
}