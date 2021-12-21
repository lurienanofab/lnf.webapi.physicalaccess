using LNF.DataAccess;
using System;
using System.Web.Http;

namespace LNF.WebApi.PhysicalAccess
{
    /// <summary/>
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IDisposable _uow;

        /// <summary/>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        /// <summary/>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _uow = WebApiConfig.WebApp.Context.GetInstance<IDataAccessService>().StartUnitOfWork();
        }

        /// <summary/>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (_uow != null)
                _uow.Dispose();
        }
    }
}
