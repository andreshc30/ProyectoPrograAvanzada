using System.Web;
using System.Web.Optimization;

namespace ProyectoPA
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Los recursos del sitio viven en ~/assets (se referencian directamente en _Layout.cshtml)
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/libs/jquery/jquery-3.7.0.min.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/assets/libs/bootstrap/bootstrap.bundle.min.js"));
        }
    }
}
