using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unibim.Melanom.WebUI.Models.Toastr
{
    public static class ToastrService
    {
        private static readonly List<(DateTime Date, int SessionId, Toastr Toastr)> _toastrs =
            new List<(DateTime Date, int SessionId, Toastr Toastr)>();

        private static int GetSessionId()
        {
            Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici y = (Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici)HttpContext.Current.Session["Yonetici"];
            Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici b = (Unibim.Melanom.WebUI.Models.EntityModel.YetkiliKullanici)HttpContext.Current.Session["Bayi"];
            int sessionID;
            if (y == null && b == null)
            {
                sessionID = 0;
            }
            if (y != null)
            {
                sessionID = y.kullaniciId;
            }
            else if (b != null)
            {
                sessionID = b.kullaniciId;
            }
            else
            {
                sessionID = 0;

            }
            return sessionID;
        }

        public static void AddToUserQueue(Toastr toastr)
        {
            _toastrs.Add((Date: DateTime.Now, SessionId: GetSessionId(), Toastr: toastr));
        }

        public static void AddToUserQueue(string message, string title, ToastrType type)
        {
            AddToUserQueue(new Toastr(message, title, type));
        }

        public static bool HasUserQueue()
        {
            int sessionId = GetSessionId();
            return _toastrs.Any(x => x.SessionId == sessionId);
        }

        public static void RemoveUserQueue()
        {
            int sessionId = GetSessionId();
            _toastrs.RemoveAll(x => x.SessionId == sessionId);
        }

        public static void ClearAll()
        {
            _toastrs.Clear();
        }

        public static List<(DateTime Date, int SessionId, Toastr Toastr)> ReadUserQueue()
        {
            int sessionId = GetSessionId();
            return _toastrs.Where(x => x.SessionId == sessionId).OrderBy(x => x.Date).ToList();
        }

        public static List<(DateTime Date, int SessionId, Toastr Toastr)> ReadAndRemoveUserQueue()
        {
            var list = ReadUserQueue();
            RemoveUserQueue();

            return list;
        }

        public static string ToJavascript(List<(DateTime Date, int SessionId, Toastr Toastr)> list)
        {
            var toastrsJsStrings = list.Select(x => x.Toastr.ToJavascript());
            return string.Join("", toastrsJsStrings);
        }
    }
}