using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Unibim.Melanom.WebUI.Models;
using Newtonsoft.Json;

namespace Unibim.Melanom.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Hasta hastaJson = new Hasta();
            //hastaJson.yas = 20;
            //hastaJson.cinsiyet = 1;
            //hastaJson.yuzey = 4;
            //hastaJson.boyut = 2;

            ////var json = new JavaScriptSerializer().Serialize(hastaJson);
            //var json = JsonConvert.SerializeObject(hastaJson);
            //var response = JsonConvert.DeserializeObject<HastaResult>(new Helper().JsonPostData(Helper.PostUrl, json));

            //var sonuc = new Helper().JsonPostFile();
            return View();
        }
    }
}