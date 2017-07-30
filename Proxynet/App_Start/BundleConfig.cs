using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Proxynet
{
    [ExcludeFromCodeCoverage]
    public class BundleConfig
    {
        private const string AdminAppDir = "App";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/angular-messages.js"));

            bundles.Add(new ScriptBundle("~/bundles/lodash").Include(
                "~/Scripts/lodash.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/systemjs").Include("~/node_modules/systemjs/dist/system.js"));

            AddApplicationBundles(bundles);
            //BundleTable.VirtualPathProvider = new ScriptBundlePathProvider(HostingEnvironment.VirtualPathProvider);

            //BundleTable.EnableOptimizations = true;
        }

        #region  ///  ScriptBundlePathProvider  ///
        private class ScriptBundlePathProvider : VirtualPathProvider
        {
            private readonly VirtualPathProvider _virtualPathProvider;

            public ScriptBundlePathProvider(VirtualPathProvider virtualPathProvider)
            {
                _virtualPathProvider = virtualPathProvider;
            }

            public override bool FileExists(string virtualPath)
            {
                return _virtualPathProvider.FileExists(virtualPath);
            }

            public override VirtualFile GetFile(string virtualPath)
            {
                return _virtualPathProvider.GetFile(virtualPath);
            }

            public override VirtualDirectory GetDirectory(string virtualDir)
            {
                return _virtualPathProvider.GetDirectory(virtualDir);
            }

            public override bool DirectoryExists(string virtualDir)
            {
                return _virtualPathProvider.DirectoryExists(virtualDir);
            }
        }
        #endregion

        #region  ///  AddApplicationBundles  ///
        private static void AddApplicationBundles(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/app");
            scriptBundle
                .IncludeDirectory("~/build", "typescript*", false);
            //var adminAppDirFullPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}", AdminAppDir));
            //if (Directory.Exists(adminAppDirFullPath))
            //{
            //    scriptBundle.Include(
            //            //  Order matters, so get the core app setup first..
            //            //---------------------------------------------------
            //            string.Format("~/{0}/module.js", AdminAppDir))

            //        //  then get any other top level js files..
            //        //---------------------------------------------------
            //        .IncludeDirectory(string.Format("~/{0}", AdminAppDir), "*.js", false)

            //        //  then get all nested module js files..
            //        //---------------------------------------------------
            //        .IncludeDirectory(string.Format("~/{0}", AdminAppDir), "*.module.js", true)

            //        //  finally, get all the other js files
            //        //---------------------------------------------------
            //        .IncludeDirectory(string.Format("~/{0}", AdminAppDir), "*.js", true);
            //}
            bundles.Add(scriptBundle);
            //bundles.Add(new StyleBundle("~/app-styles").Include(
            //    "~/Content/css/site.css"));
        }
        #endregion
    }
}
