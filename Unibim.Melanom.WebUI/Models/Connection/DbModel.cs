using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.Connection
{
    public class DbModel
    {
        public static ModelContext EF
        {
            get
            {
                if (HttpContext.Current.Session["_ContextEF"] == null)
                    HttpContext.Current.Session["_ContextEF"] = new ModelContext();
                return (ModelContext)HttpContext.Current.Session["_ContextEF"];
            }
        }

        public static ModelContext EFRefresh
        {
            get
            {
                HttpContext.Current.Session["_ContextEF"] = new ModelContext();
                return (ModelContext)HttpContext.Current.Session["_ContextEF"];
            }
        }
    }
}