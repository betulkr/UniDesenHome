using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unibim.Melanom.WebUI.Filtre
{
    public class SkipMyGlobalActionFilterAttribute : Attribute
    {
    }

    public class _SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Actionda login kontrolünü devre dışı bırakmak için
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipMyGlobalActionFilterAttribute), false).Any())
            {
                return;
            }
            if (HttpContext.Current.Session["Yonetici"] == null && HttpContext.Current.Session["Bayi"] == null && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Kurumsal")
            {
                //filterContext.Result = new RedirectResult("/Site/Home");
                return;
            }
            else
            {
                if (((filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "MusteriSiparis") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Urun") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "OzellikTur") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Takim") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Yonetici") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "AltKategori") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "BayiIslem") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Kategori") || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Musteri")) && HttpContext.Current.Session["Yonetici"] == null)
                {

                    filterContext.Result = new RedirectResult("/Login/Index");
                    return;
                }
                else if (HttpContext.Current.Session["Bayi"] == null && (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Bayi"  || filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Siparis" /*|| filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "SiteManage" || filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Manager"*/))
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                    return;
                }
                else
                {
                    //filterContext.Result = new RedirectResult(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName);
                    return;
                }
            }
        }
    }
}