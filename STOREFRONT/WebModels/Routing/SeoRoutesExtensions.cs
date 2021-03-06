﻿#region

using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Routing.Constraints;
using VirtoCommerce.Web.Models.Routing.Routes;

#endregion

namespace VirtoCommerce.Web.Models.Routing
{
    public static class SeoRoutesExtensions
    {
        #region Public Methods and Operators
        public static void MapSeoRoutes(this RouteCollection routes)
        {
            var pageRoute =
                new NormalizeRoute(
                    new PageRoute(
                        Constants.PageRoute,
                        new RouteValueDictionary
                        {
                            { "controller", "Page" },
                            { "action", "DisplayPageAsync" },
                            //{ Constants.Language, UrlParameter.Optional }

                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Page, new PageRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var itemRoute =
                new NormalizeRoute(
                    new ItemRoute(
                        Constants.ItemRoute,
                        new RouteValueDictionary
                        {
                            { "controller", "Product" },
                            { "action", "ProductByKeywordAsync" },
                            { Constants.Language, "" },
                            { Constants.Store, "" }

                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Category, new CategoryRouteConstraint() },
                            { Constants.Item, new ItemRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var categoryRoute =
                new NormalizeRoute(
                    new CategoryRoute(
                        Constants.CategoryRoute,
                        new RouteValueDictionary
                        {
                            { "controller", "Collections" },
                            { "action", "GetCollectionByKeywordAsync" },
                            //{ Constants.Language, UrlParameter.Optional },
                            { Constants.Tags, UrlParameter.Optional },
                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Category, new CategoryRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var storeRoute =
                new NormalizeRoute(
                    new StoreRoute(
                        Constants.StoreRoute,
                        new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            //Legacy redirects
            routes.Redirect(r => r.MapRoute("old_Collection", string.Format("collections/{{{0}}}", Constants.Category))).To(categoryRoute,
                x =>
                {
                    //Expect to receive category code
                    if (x.RouteData.Values.ContainsKey(Constants.Category))
                    {
                        var client = ClientContext.Clients.CreateBrowseClient();
                        var store = SiteContext.Current.Shop.StoreId;
                        var language = SiteContext.Current.Language;
                        var category = Task.Run(() => client.GetCategoryByCodeAsync(store, language, x.RouteData.Values[Constants.Category].ToString())).Result;
                        if (category != null)
                        {
                            var model = category.AsWebModel();
                            return new RouteValueDictionary { { Constants.Category, model.Outline } };
                        }
                    }
                    return null;
                });


            var defaultRoute = new NormalizeRoute(
                new Route(string.Format("{0}/{{controller}}/{{action}}/{{id}}", Constants.StoreRoute),
                    new RouteValueDictionary { 
                    { "id", UrlParameter.Optional }, 
                    { "action", "Index" } 
                    },
                    new RouteValueDictionary
                    {
                        { Constants.Language, new LanguageRouteConstraint() },
                        { Constants.Store, new StoreRouteConstraint() }
                    },
                    new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                    new MvcRouteHandler()));

            routes.Add("Page", pageRoute);
            routes.Add("Item", itemRoute);
            routes.Add("Category", categoryRoute);
            routes.Add("Store", storeRoute);

            //Other actions
            routes.Add("RelativeDefault", defaultRoute);
        }
        #endregion
    }
}