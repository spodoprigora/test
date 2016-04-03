using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using test.ViewModel;

namespace test
{
    public static class PagingHelper
    {
        public const int FirtsPageIndex = 1;
        private const int AmountOfControlsBeforeDots = 4;
        private const int AmountOfPageControlsOnLeftRightFromCurrent = 2;
        private const int MaxAnountOfPageControls = 9;
        private const string DotsControlHtml = "<a>...</a>";
        private const string WhiteSpace = " ";
        private const string Action = "action";
        private const string PreviousPageControlClass = "prev";
        private const string NextPageControlClass = "next";
        private const string CurrentPageControlClass = "current";
        private const string DisabledControlClass = "disable";
        private const string PreviousPageLinkText = "";
        private const string NextPageLinkText = "";
        private const string PageQueryParameterName = "page";

        public enum PageLinkOption
        {
            Next,
            Prevoius,
            First,
            Last
        }

        /// <summary>
        ///     Returns a full pagination block
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="queryStringValues"></param>
        /// <param name="previousPageLinkText"></param>
        /// <param name="nextPageLinkText"></param>
        /// <param name="pageQueryParameterName"></param>
        /// <param name="firtsPageIndex"></param>
        /// <param name="maxAnountOfPageControls"></param>
        /// <param name="amountOfPageControlsOnLeftRightFromCurrent"></param>
        /// <param name="dotsControlHtml"></param>
        /// <param name="previousPageControlClass"></param>
        /// <param name="nextPageControlClass"></param>
        /// <param name="currentPageControlClass"></param>
        /// <param name="disabledControlClass"></param>
        /// <returns></returns>
        public static MvcHtmlString PaginationBlock(this HtmlHelper htmlHelper, PagingModel pagingInfo,
            NameValueCollection queryStringValues, AjaxHelper ajaxHelper = null, AjaxOptions ajaxOptions = null,
            string previousPageLinkText = PreviousPageLinkText, string nextPageLinkText = NextPageLinkText,
            string pageQueryParameterName = PageQueryParameterName,
            int firtsPageIndex = FirtsPageIndex, int maxAnountOfPageControls = MaxAnountOfPageControls,
            int amountOfPageControlsOnLeftRightFromCurrent = AmountOfPageControlsOnLeftRightFromCurrent,
            string dotsControlHtml = DotsControlHtml, string previousPageControlClass = PreviousPageControlClass,
            string nextPageControlClass = NextPageControlClass,
            string currentPageControlClass = CurrentPageControlClass, string disabledControlClass = DisabledControlClass)
        {
            var previousClass = pagingInfo.CurrentPage - 1 > 0
                ? previousPageControlClass
                : string.Concat(previousPageControlClass, WhiteSpace, disabledControlClass);
            var nextClass = pagingInfo.CurrentPage != pagingInfo.TotalPages
                ? nextPageControlClass
                : string.Concat(nextPageControlClass, WhiteSpace, disabledControlClass);
            var lastClass = pagingInfo.CurrentPage == pagingInfo.TotalPages
                ? currentPageControlClass
                : string.Empty;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var sb = new StringBuilder();

            var previousPageControl = htmlHelper.PageLinks(urlHelper, PageLinkOption.Prevoius, pagingInfo,
                queryStringValues, ajaxHelper, ajaxOptions, previousPageLinkText, new { @class = @previousClass },
                pageQueryParameterName);
            sb.Append(previousPageControl);

            if (pagingInfo.TotalPages < maxAnountOfPageControls)
            {
                for (var i = firtsPageIndex; i < pagingInfo.TotalPages + firtsPageIndex; i++)
                {
                    if (pagingInfo.CurrentPage == i)
                    {
                        var pageLink = htmlHelper.PageLinks(urlHelper, i, queryStringValues,
                            i.ToString(CultureInfo.InvariantCulture), ajaxHelper, ajaxOptions,
                            new { @class = currentPageControlClass }, pageQueryParameterName);
                        sb.Append(pageLink);
                    }
                    else
                    {
                        var pageLink = htmlHelper.PageLinks(urlHelper, i, queryStringValues,
                            i.ToString(CultureInfo.InvariantCulture), ajaxHelper, ajaxOptions, pageQueryParameterName);
                        sb.Append(pageLink);
                    }
                }
            }
            else
            {
                if (pagingInfo.CurrentPage >= AmountOfControlsBeforeDots)
                {
                    var pageLink = htmlHelper.PageLinks(urlHelper, PageLinkOption.First, pagingInfo, queryStringValues,
                        ajaxHelper, ajaxOptions, firtsPageIndex.ToString(CultureInfo.InvariantCulture),
                        pageQueryParameterName);
                    sb.Append(pageLink);
                    sb.Append(dotsControlHtml);
                }

                for (var i = pagingInfo.CurrentPage - amountOfPageControlsOnLeftRightFromCurrent;
                    i <= pagingInfo.CurrentPage + amountOfPageControlsOnLeftRightFromCurrent;
                    i++)
                {
                    if (i < firtsPageIndex || i >= pagingInfo.TotalPages)
                    {
                        continue;
                    }

                    if (i == pagingInfo.CurrentPage)
                    {
                        var pageLink = htmlHelper.PageLinks(urlHelper, i, queryStringValues,
                            i.ToString(CultureInfo.InvariantCulture), ajaxHelper, ajaxOptions,
                            new { @class = currentPageControlClass }, pageQueryParameterName);
                        sb.Append(pageLink);
                    }
                    else
                    {
                        var pageLink = htmlHelper.PageLinks(urlHelper, i, queryStringValues,
                            i.ToString(CultureInfo.InvariantCulture), ajaxHelper, ajaxOptions, pageQueryParameterName);
                        sb.Append(pageLink);
                    }
                }

                if (pagingInfo.TotalPages - pagingInfo.CurrentPage >= AmountOfControlsBeforeDots)
                {
                    sb.Append(dotsControlHtml);
                }

                var lastPageLink = htmlHelper.PageLinks(urlHelper, PageLinkOption.Last, pagingInfo, queryStringValues,
                    ajaxHelper, ajaxOptions, pagingInfo.TotalPages.ToString(CultureInfo.InvariantCulture),
                    new { @class = @lastClass },
                    pageQueryParameterName);
                sb.Append(lastPageLink);
            }

            var nextPageLink = htmlHelper.PageLinks(urlHelper, PageLinkOption.Next, pagingInfo, queryStringValues,
                ajaxHelper, ajaxOptions, nextPageLinkText, new { @class = @nextClass }, pageQueryParameterName);
            sb.Append(nextPageLink);

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        ///     return link for dedicated page option
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="urlHelper"></param>
        /// <param name="pageOption"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="queryStringValues"></param>
        /// <param name="linkText"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="pageParameterName"></param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper, UrlHelper urlHelper, PageLinkOption pageOption,
            PagingModel pagingInfo, NameValueCollection queryStringValues, AjaxHelper ajaxHelper = null,
            AjaxOptions ajaxOptions = null,
            string linkText = "", object htmlAttributes = null, string pageParameterName = PageQueryParameterName,
            int firtsPageIndex = FirtsPageIndex)
        {
            var url = urlHelper.PageUrl(pageOption, pagingInfo, queryStringValues, pageParameterName, firtsPageIndex);

            if (string.IsNullOrEmpty(url))
            {
                return MvcHtmlString.Empty;
            }
            if (ajaxHelper != null && ajaxOptions != null)
            {
                ajaxOptions.Url = url;
                var actionName = urlHelper.RequestContext.RouteData.Values[Action].ToString();
                var link = ajaxHelper.ActionLink(linkText, actionName, null, ajaxOptions, htmlAttributes);
                return link;
            }


            //if (ajaxHelper != null && ajaxOptions != null)
            //{
            //    return ajaxHelper.ActionLink(linkText, urlHelper.RequestContext.RouteData.Values[Action].ToString(),
            //        queryStringValues, ajaxOptions, htmlAttributes);
            //}

            var attributes = GetHtmlAttributes(htmlAttributes);

            var tagBuilder = new TagBuilder("a");

            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.SetInnerText(linkText);

            //SetAjaxAttributes(ajaxOptions, tagBuilder, url);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        ///     return link for dedicated page number
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="urlHelper"></param>
        /// <param name="page"></param>
        /// <param name="queryStringValues"></param>
        /// <param name="linkText"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="pageParameterName"></param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper, UrlHelper urlHelper, int page,
            NameValueCollection queryStringValues, string linkText, AjaxHelper ajaxHelper = null,
            AjaxOptions ajaxOptions = null,
            object htmlAttributes = null, string pageParameterName = PageQueryParameterName)
        {
            var url = urlHelper.PageUrl(page, queryStringValues, pageParameterName);

            if (string.IsNullOrEmpty(url))
            {
                return MvcHtmlString.Empty;
            }

            if (ajaxHelper != null && ajaxOptions != null)
            {
                ajaxOptions.Url = url;
                var actionName = urlHelper.RequestContext.RouteData.Values[Action].ToString();
                var link = ajaxHelper.ActionLink(linkText, actionName, null, ajaxOptions, htmlAttributes);
                return link;
            }
            //if (ajaxHelper != null && ajaxOptions != null)
            //{
            //    return ajaxHelper.ActionLink(linkText, urlHelper.RequestContext.RouteData.Values[Action].ToString(),
            //        queryStringValues, ajaxOptions, htmlAttributes);
            //}

            var attributes = GetHtmlAttributes(htmlAttributes);
            var tagBuilder = new TagBuilder("a");

            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.SetInnerText(linkText);

            //SetAjaxAttributes(ajaxOptions, tagBuilder, url);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        ///     return url for dedicated page option
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="pageOption"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="queryStringValues"></param>
        /// <param name="pageParameterName"></param>
        /// <param name="firstPageIndex"></param>
        /// <returns></returns>
        public static string PageUrl(this UrlHelper urlHelper, PageLinkOption pageOption, PagingModel pagingInfo,
            NameValueCollection queryStringValues, string pageParameterName = PageQueryParameterName,
            int firstPageIndex = FirtsPageIndex)
        {
            switch (pageOption)
            {
                case PageLinkOption.Prevoius:
                    var previousPageNumber = firstPageIndex;
                    if (pagingInfo.CurrentPage > firstPageIndex)
                    {
                        previousPageNumber = pagingInfo.CurrentPage - 1;
                    }
                    return CreateUrl(urlHelper, queryStringValues, pageParameterName, previousPageNumber);

                case PageLinkOption.Next:
                    var nextPageNumber = pagingInfo.CurrentPage;
                    if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
                    {
                        nextPageNumber = pagingInfo.CurrentPage + 1;
                    }
                    return CreateUrl(urlHelper, queryStringValues, pageParameterName, nextPageNumber);

                case PageLinkOption.First:

                    return CreateUrl(urlHelper, queryStringValues, pageParameterName, firstPageIndex);

                case PageLinkOption.Last:

                    return CreateUrl(urlHelper, queryStringValues, pageParameterName, pagingInfo.TotalPages);

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///     return url for dedicated page number
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="page"></param>
        /// <param name="queryStringValues"></param>
        /// <param name="pageParameterName"></param>
        /// <returns></returns>
        public static string PageUrl(this UrlHelper urlHelper, int page, NameValueCollection queryStringValues,
            string pageParameterName = PageQueryParameterName)
        {
            return CreateUrl(urlHelper, queryStringValues, pageParameterName, page);
        }

        private static IDictionary<string, string> GetHtmlAttributes(object htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return new Dictionary<string, string>();
            }

            var attributes = new Dictionary<string, string>();
            var props = htmlAttributes.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var name = propertyInfo.Name.Replace('_', '-');
                var value = propertyInfo.GetValue(htmlAttributes);
                attributes.Add(name, value.ToString());
            }
            return attributes;
        }

        private static string CreateUrl(UrlHelper urlHelper, NameValueCollection queryStringValues,
            string pageParameterName, int followingPageNumber)
        {
            var routeValue = new RouteValueDictionary();
            for (var i = 0; i < queryStringValues.Count; i++)
            {
                var key = queryStringValues.AllKeys[i];
                var value = queryStringValues[i];
                if (string.Equals(key, pageParameterName, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                if (!routeValue.ContainsKey(key))
                {
                    routeValue.Add(key, value);
                }
            }
            routeValue.Add(pageParameterName, followingPageNumber);

            var link = urlHelper.Action(urlHelper.RequestContext.RouteData.Values[Action].ToString(), routeValue);

            return link;
        }
    }
}