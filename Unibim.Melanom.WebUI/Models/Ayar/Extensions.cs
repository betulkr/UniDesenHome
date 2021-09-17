using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Drawing;

// mvc form elemanlarına yenilerinin eklenmesi
namespace Unibim.Melanom.WebUI
{
    public static class Extensions
    {
        public static MvcForm UiForm(this AjaxHelper ajaxHelper, string formId, string controllerName, string actionName, object routeValues, string onSuccessScript)
        {
            return ajaxHelper.BeginForm(actionName, controllerName, null, new AjaxOptions { OnBegin = "OnBegin", OnComplete = "OnComplete", OnFailure = "OnFailure", OnSuccess = onSuccessScript }, new { id = formId });
        }

        public static MvcHtmlString UiHidden(this HtmlHelper htmlHelper, string name)
        {
            return htmlHelper.Hidden(name, string.Empty);
        }
        public static MvcHtmlString UiHidden(this HtmlHelper htmlHelper, string name, string value)
        {
            return htmlHelper.Hidden(name, value);
        }

        
        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                if (viewResult.View == null)
                {
                    throw new InvalidOperationException(
                    string.Format("The partial view '{0}' could not be found", viewName));
                }


                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static MvcHtmlString StrajaxPager(this HtmlHelper htmlHelper, int totalRecord, int currentPage, int take, string tableId)
        {
            return new MvcHtmlString(CreatePager(totalRecord, currentPage, take, tableId));
        }

        public static string StrajaxPager(int totalRecord, int currentPage, int take, string tableId)
        {
            return CreatePager(totalRecord, currentPage, take, tableId);
        }

        private static string CreatePager(int totalRecord, int currentPage, int take, string tableId)
        {
            string gridInfo;

            if (totalRecord == 0 && take == 0) // TODO : Divide by zero
            {
                totalRecord = 1;
                take = 1;
            }
            // TODO: Resources!!!
            int totalPage = (totalRecord - (totalRecord % take)) / take;
            if (totalRecord % take > 0)
            {
                totalPage++;
            }

            if (totalRecord != 0)
            {
                gridInfo = string.Format("Kayıt", currentPage + 1, totalPage, totalRecord);
            }
            else
            {
                gridInfo = string.Format("Kayıt", 0, totalPage, totalRecord);
            }

            StringBuilder sbPager = new StringBuilder();
            sbPager.AppendLine(string.Format("<div id=\"pager_{0}\">", tableId));
            sbPager.AppendLine(string.Format("<div class=\"fg-toolbar tableFooter\" data-tableid=\"{0}\">", tableId));
            sbPager.AppendLine(string.Format("<div class=\"dataTables_info\"> {0} </div>", gridInfo));
            sbPager.AppendLine("<div class=\"dataTables_paginate paging_full_numbers\">");
            sbPager.AppendLine("<span>");
            if (currentPage == 0)
            {
                sbPager.AppendLine(string.Format("<a class=\"first paginate_button paginate_button_disabled\">{0}</a>", "Önceki"));
                sbPager.AppendLine(string.Format("<a class=\"previous paginate_button paginate_button_disabled\">{0}</a>", "Sonraki"));
            }
            else
            {
                sbPager.AppendLine(string.Format("<a data-item_page=\"{0}\" class=\"first paginate_button\">{1}</a>", 0, "İlk"));
                sbPager.AppendLine(string.Format("<a data-item_page=\"{0}\" class=\"previous paginate_button\">{1}</a>", (currentPage - 1), "Önceki"));
            }

            int firstPage = currentPage - 2;

            if (firstPage < 0)
            {
                firstPage = 0;
            }

            int lastPage = firstPage + 5;
            if (lastPage > totalPage)
            {
                lastPage = totalPage;
            }

            for (int i = firstPage; i < lastPage;)
            {
                int pageName = i + 1;
                if (i == currentPage)
                {
                    sbPager.AppendLine(string.Format("<a class=\"paginate_active\">{0}</a>", pageName));
                }
                else
                {
                    sbPager.AppendLine(string.Format("<a data-item_page=\"{1}\" class=\"paginate_button\">{0}</a>", pageName, i));
                }
                i++;
            }

            if (currentPage >= totalPage - 1)
            {
                sbPager.AppendLine(string.Format("<a class=\"next paginate_button paginate_button_disabled\">{0}</a>", "Sonraki"));
                sbPager.AppendLine(string.Format("<a class=\"last paginate_button paginate_button_disabled\">{0}</a>", "Son"));
            }
            else
            {
                sbPager.AppendLine(string.Format("<a data-item_page=\"{0}\" class=\"next paginate_button\">{1}</a>", (currentPage + 1), "İlk"));
                sbPager.AppendLine(string.Format("<a data-item_page=\"{0}\" class=\"last paginate_button\">{1}</a>", (totalPage - 1),"Son"));
            }

            sbPager.AppendLine("</span>");
            sbPager.AppendLine("</div>");
            sbPager.AppendLine("</div>");
            sbPager.AppendLine("</div>");
            return sbPager.ToString();
        }

       
        public static string ThemeName(this HtmlHelper htmlHelper)
        {
            //TODO: Setting Table...
            return "aquincum";

        }


        public static string TableFormCollectionValue(this FormCollection formCollection, string key)
        {
            string value = string.Empty;
            if (formCollection["fc"] != null)
            {
                foreach (var item in formCollection["fc"].Split('&')) // fc = xxx=1&yyy=2...
                {
                    if (key == item.Split('=')[0].ToString())
                    {
                        value = item.Split('=')[1].ToString();
                    }
                }
            }
            return value;
        }

        

        public static MvcHtmlString SurveyQuestionInput(this HtmlHelper helper, string name, string id, string text, int surveyTypeNo, bool isRequired)
        {
            string tag = string.Empty;
            tag = "<div class='field'>";

            switch (surveyTypeNo)
            {
                case 1:
                    tag += string.Format(@"<input type='checkbox' name='q_{0}' id='q_{1}' class='check' value='{1}' />
                                           <label for='q_{1}'>{2}</label><span class='spam'>{3}</span>",
                                            name,
                                            id,
                                            text,
                                            isRequired ? " *" : "");
                    break;
                case 2:
                    tag += string.Format(@"<input type='radio' name='q_{0}' id='q_{1}' value='{1}' />
                                           <label for='q_{1}'>{2}</label><span class='spam'>{3}</span>",
                                            name,
                                            id,
                                            text,
                                            isRequired ? " *" : "");
                    break;
                case 3:
                    tag += string.Format(@"<span>{2}</span><span class='spam'>{3}</span>",
                                            name,
                                            id,
                                            text,
                                            isRequired ? " *" : "");
                    break;
                case 4:
                    tag += string.Format("<input type='text' name='t_{0}' id='t_{1}' placeholder='{2}' /><span class='spam'>{3}</span>",
                                            //name,
                                            id,
                                            id,
                                            text,
                                            isRequired ? " *" : "");
                    break;
            }
            tag += "</div>";
            return MvcHtmlString.Create(tag);
        }

        public static MvcHtmlString File(this HtmlHelper html, string name)
        {
            var tb = new TagBuilder("input");
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("name", name);
            tb.GenerateId(name);
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString FileFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            string name = GetFullPropertyName(expression);
            return html.File(name);
        }

        #region Helpers

        static string GetFullPropertyName<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            MemberExpression memberExp;

            if (!TryFindMemberExpression(exp.Body, out memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();

            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        static bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;

            if (memberExp != null)
                return true;

            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;

                if (memberExp != null)
                    return true;
            }

            return false;
        }

        static bool IsConversion(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked);
        }

        #endregion
    }



}