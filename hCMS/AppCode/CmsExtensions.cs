using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CMSLib;

namespace hCMS.Library
{
    public static class CmsExtensions
    {
        /// <summary>
        /// Kiểm tra List có giá trị hay không
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool HasValue<T>(this List<T> items)
        {
            if (items != null)
            {
                if (items.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string TrimmedOrDefault(this string str, string strDefault)
        {
            return string.IsNullOrEmpty(str) ? strDefault : str.Trim();
        }

        public static List<Actions> GetActionsListByParentId(this List<Actions> list, short parentActionId)
        {
            List<Actions> retVal = new List<Actions>();
            foreach (Actions action in list)
            {
                if (action.ParentActionId == parentActionId && action.Display > 0)
                {
                    retVal.Add(action);
                }
            }
            return retVal;
        }

        public static RoleActions RoleActionsGetByRoleId(this List<RoleActions> list, short roleId)
        {
            if (list.HasValue())
            {
                return list.FirstOrDefault(m => m.RoleId == roleId);
            }
            return new RoleActions();
        }

        //public static Actions ActionsGetByActionId(this List<Actions> list, short actionId)
        //{
        //    if (list.HasValue())
        //    {
        //        return list.FirstOrDefault(m => m.ActionId == actionId);
        //    }
        //    return new Actions();
        //}

        public static string GetLinkPage(int page = 1)
        {
            string rawUrl = HttpContext.Current.Request.RawUrl;
            if (string.IsNullOrEmpty(rawUrl))
            {
                return rawUrl;
            }
            rawUrl = Regex.Replace(rawUrl, @"[?|&]page=[0-9]+", string.Empty);
            return rawUrl.Contains("?") ? rawUrl + "&page=" + page : rawUrl + "?page=" + page;
        }

        public static IEnumerable<SelectListItem> AddDefaultOption(this IEnumerable<SelectListItem> list, string dataTextField, string selectedValue)
        {
            var items = new List<SelectListItem> { new SelectListItem() { Text = dataTextField, Value = selectedValue } };
            items.AddRange(list);
            return items;
        }

    }
}