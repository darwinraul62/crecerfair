using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Threading;
using Weavy.Web.Owin;

[assembly: OwinStartup(typeof(Weavy.Startup))]
namespace Weavy {

    /// <summary>
    /// OWIN Startup class that configures our application on application start.
    /// </summary>
    public partial class Startup {

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app) {
            app.UseWeavy();
            
            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
        }
    }
}
