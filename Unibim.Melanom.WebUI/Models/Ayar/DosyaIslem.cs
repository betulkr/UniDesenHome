using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Unibim.Melanom.WebUI
{
    public class DosyaIslem
    {
        public string ResimEkle(HttpPostedFileBase resim, string yol,int w,int h)
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
                        res.Save(yol + resimAd);
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
                    System.IO.File.Delete(yol + resim);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}