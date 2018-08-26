using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;



namespace CH5_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(string reauth)
        {
            /*
            string clientId = ConfigurationManager.AppSettings["ClientId"];

            string tenantID = ConfigurationManager.AppSettings["Tenant"];

            string PostLogoutRedirectUri = ConfigurationManager.AppSettings["RedirectUrl"];

            string authority = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Authority"], tenantID);

            string appKey = ConfigurationManager.AppSettings["appKey"];

            string graphResourceId = "https://graph.windows.net";



            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

           
                ClientCredential credential = new ClientCredential(clientId, appKey);
            AuthenticationContext authContext = new AuthenticationContext(authority);//, new WebSessionCache(userObjectID));
            AuthenticationResult result = authContext.AcquireTokenSilent(graphResourceId, credential, UserIdentifier.AnyUser);
            //, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            //AuthenticationResult result = authContext.AcquireTokenSilent(graphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            
            HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response =

                httpClient.GetAsync("https://graph.windows.net/me?api-version=1.6").Result;




                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = response.Content.ReadAsStringAsync().Result;
                }
                */

              
                if (reauth == null)

                {
                    string clientId = ConfigurationManager.AppSettings["ClientId"];

                    string tenantID = ConfigurationManager.AppSettings["Tenant"];

                    string PostLogoutRedirectUri = ConfigurationManager.AppSettings["RedirectUrl"];

                    string authority = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Authority"], tenantID);

                    string appKey = ConfigurationManager.AppSettings["appKey"];

                    string graphResourceId = "https://graph.windows.net";



                    string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                    try
                    {
                        ClientCredential credential = new ClientCredential(clientId, appKey);
                        AuthenticationContext authContext = new AuthenticationContext(authority, new WebSessionCache(userObjectID));
                        AuthenticationResult result = authContext.AcquireTokenSilent(graphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                        HttpClient httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                        HttpResponseMessage response =

                        httpClient.GetAsync("https://graph.windows.net/me?api-version=1.6").Result;




                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.Message = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (AdalException ex)
                    {

                        if (ex.ErrorCode == "failed_to_acquire_token_silently")
                        {
                            Response.Write("<a href=\"./About?reauth=true\">Your tokens are expired. Click here to reauth </ a > ");
    }
                        else
                        {
                            // more error handling
                        }
                    }


                }
                else
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/Home/About", },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }
               
                return View();
            
        }

        
        /// <summary>
        /// [Authorize]标记用于保护访问的资源
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Contact()
        {
            //string userFirstName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName).Value;

   
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            //获取用户的You get the user’s first and last name below:
            ViewBag.Name = userClaims?.FindFirst("name")?.Value;

            // The 'Name' claim can be used for showing the username
            ViewBag.Username = userClaims?.FindFirst(System.IdentityModel.Claims.ClaimTypes.Name)?.Value;

            // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // TenantId is the unique Tenant Id - which represents an organization in Azure AD
            ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;


            string userName = ClaimsPrincipal.Current.Identity.Name;
            ViewBag.Message = String.Format("Welcome, {0}!", userName);

            return View();
        }

       
    }
}