using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Utility
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString ValidatorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var prop = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            var name = ExpressionHelper.GetExpressionText(expression);
            return ValidatorInternal(html, name, prop.Description);
        }

        public static MvcHtmlString Validator(this HtmlHelper html, ModelMetadata prop)
        {
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(prop.PropertyName);
            return ValidatorInternal(html, name, prop.Description);
        }

        public static MvcHtmlString Validator(this HtmlHelper html, string name, string description = null)
        {
            return ValidatorInternal(html, name, description);
        }

        static MvcHtmlString ValidatorInternal(this HtmlHelper html, string name, string description)
        {
            if (html.ViewData.ModelState.IsValidField(name))
            {
                if (!String.IsNullOrWhiteSpace(description))
                {
                    var help = UrlHelper.GenerateContentUrl("~/Content/Images/help.png", html.ViewContext.HttpContext);
                    TagBuilder img = new TagBuilder("img");
                    img.Attributes.Add("src", help);
                    img.Attributes.Add("title", description);
                    return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
                }
            }
            else
            {
                var error = UrlHelper.GenerateContentUrl("~/Content/Images/error.png", html.ViewContext.HttpContext);
                TagBuilder img = new TagBuilder("img");
                img.AddCssClass("error");
                img.Attributes.Add("src", error);
                var title = html.ViewData.ModelState[name].Errors.First().ErrorMessage;
                if (!String.IsNullOrWhiteSpace(description)) title += "\n\n" + description;
                img.Attributes.Add("title", title);
                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }

            return MvcHtmlString.Empty;
        }

        public static string RelyingParty(this HtmlHelper helper, string path)
        {
            var replyTo = "https://bringsy.com/"; // read from default value configuration
            var cooky = HttpContext.Current.Request.Cookies["idsrvlr"];
            if (cooky != null)
            {
                var lastUsedRealm = cooky.Value;
                if (!string.IsNullOrEmpty(lastUsedRealm))
                {
                    var authHelper = new AuthenticationHelper();
                    var rp = authHelper.GetRelyingPartyDetails(lastUsedRealm);
                    if (rp != null)
                    {
                        replyTo = rp.ReplyTo.ToString();
                    }
                }
            }

            return string.Format("{0}/{1}", replyTo.TrimEnd('/'), path.TrimStart('/'));
        }
    }
}