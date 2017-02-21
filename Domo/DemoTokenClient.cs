using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace Domo
{
    public static class DemoTokenClient
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string AccessToken { get; set; }
        public static DateTime ExpiresAt { get; set; }
        /// <summary>
        /// Domo access token expired after 60 minutes
        /// </summary>
        public static int DomoTokenExpireInterval = 60;
        /// <summary>
        /// Domo access token expires every 1 hour so this method will generate new token if current token expired.
        /// </summary>
        /// <param name="IsRenewToken">To force generate new access token</param>
        /// <returns></returns>
        public static string GetAccessToken(bool IsRenewToken = false)
        {

            DateTime currTime = DateTime.Now;
            var min = currTime.Subtract(ExpiresAt).TotalMinutes;
            if (min > DomoTokenExpireInterval || IsRenewToken || string.IsNullOrWhiteSpace(AccessToken))
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create("https://api.domo.com/oauth/token?grant_type=client_credentials&amp;scope=data");
                objRequest.Method = WebRequestMethods.Http.Get;
                objRequest.Headers["Authorization"] = "Basic " + AppConfiguration.BasicToken;
                objRequest.Credentials = new NetworkCredential(AppConfiguration.UserName, AppConfiguration.Password);

                try
                {
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    if (Convert.ToString(objResponse.StatusCode) == "200" || Convert.ToString(objResponse.StatusCode) == "OK")
                    {
                        using (Stream responseStream = objResponse.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {

                                string json = reader.ReadToEnd();
                                var data = (JObject)JsonConvert.DeserializeObject(json);
                                AccessToken = data["access_token"].Value<string>();

                                ExpiresAt = DateTime.Now;
                                reader.Close();
                                responseStream.Close();
                            }
                        }
                        return AccessToken;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            return AccessToken;
        }
    }
}
