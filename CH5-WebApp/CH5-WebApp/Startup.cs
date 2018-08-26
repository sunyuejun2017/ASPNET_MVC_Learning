using System;
using System.Threading.Tasks;

#region OWIN引用

using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Diagnostics;

//using Microsoft.IdentityModel.Protocols.OpenIdConnect;
//using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Web;
using System.IdentityModel.Claims;
using System.Net.Http;
using System.Net.Http.Headers;


#endregion

[assembly: OwinStartup(typeof(CH5_WebApp.Startup))]

namespace CH5_WebApp
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ClientId"];

        private static string tenantID = ConfigurationManager.AppSettings["Tenant"];

        private static string PostLogoutRedirectUri = ConfigurationManager.AppSettings["RedirectUrl"];

        private static string authority = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Authority"],tenantID);

        private static string appKey = ConfigurationManager.AppSettings["appKey"];

        private static string graphResourceId = "https://graph.windows.net";

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,

                    PostLogoutRedirectUri = PostLogoutRedirectUri,


                    // 单租户，注释59-64行代码，并将web.config中的Tenant设为xxx@xxx.onmicrosoft.com即可
                    //TokenValidationParameters = new TokenValidationParameters()
                    //{
                    //    // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
                    //    // we inject our own multitenant validation logic
                    //    ValidateIssuer = false,
                    //},

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        AuthorizationCodeReceived = (context) =>
                        {
                            Debug.WriteLine("*** AuthorizationCodeReceived");
                            var code = context.Code;
                            ClientCredential credential = new ClientCredential(clientId, appKey);
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            AuthenticationContext authContext = new AuthenticationContext(authority);
                           // AuthenticationContext authContext = new AuthenticationContext(authority, new WebSessionCache(signedInUserID));
                            //, new ADALTokenCache(signedInUserID));
                            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                            code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceId);

                            Debug.WriteLine(result.AccessToken);

                            

                            return Task.FromResult(0);

                            
                            //string ClientId = "c3d5b1ad-ae77-49ac-8a86-dd39a2f91081";
                            //string Authority = "https://login.microsoftonline.com/DeveloperTenant.onmicrosoft.com";
                            //string appKey = "a3fQREiyhqpYL10OO6hfCW+xke/TyP2oIQ6vgu68eoE=";
                            //string resourceId = "https://graph.windows.net";
                            //var code = context.Code;
                            //AuthenticationContext authContext = new AuthenticationContext(Authority);
                            //ClientCredential credential = new ClientCredential(ClientId, appKey);

                            //AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(code,
                            //new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)),
                            //credential,
                            //resourceId);
                            //return Task.FromResult(0);
                        }
                    }

                }
            );
        }
    }
}
