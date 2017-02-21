using System;
using System.IO;
using System.Net;

namespace Domo
{
    public static class DomoDatesetClient
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void ExportDataset(string datasetId)
        {
            try
            {
                HttpWebRequest objRequest = ExportRequest(datasetId);
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                if (Convert.ToString(objResponse.StatusCode) == "200" || Convert.ToString(objResponse.StatusCode) == "OK")
                {
                    using (Stream responseStream = objResponse.GetResponseStream())
                    {
                        using (Stream output = File.OpenWrite(Path.Combine(AppConfiguration.OutputFilePath, datasetId + ".csv")))
                        {
                            responseStream.CopyTo(output);
                        }
                        responseStream.Close();
                    }
                }
            }
            catch (WebException we)
            {
                log.Error(we);

                using (WebResponse response = we.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    if (Convert.ToString(httpResponse.StatusCode) == "NotAcceptable")
                    {
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            //export
                            HttpWebRequest objRequest = ExportRequest(datasetId, true);
                            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                            if (Convert.ToString(objResponse.StatusCode) == "200" || Convert.ToString(objResponse.StatusCode) == "OK")
                            {
                                using (Stream responseStream = objResponse.GetResponseStream())
                                {
                                    using (Stream output = File.OpenWrite(Path.Combine(AppConfiguration.OutputFilePath, datasetId + ".csv")))
                                    {
                                        responseStream.CopyTo(output);
                                    }
                                    responseStream.Close();
                                }
                            }

                            string text = reader.ReadToEnd();
                            Console.WriteLine(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public static HttpWebRequest ExportRequest(string datasetId, bool IsRenewToken = false)
        {
            string token = DemoTokenClient.GetAccessToken(IsRenewToken);
            string datasetUrl = "https://api.domo.com/v1/datasets/" + datasetId + "/data";
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(datasetUrl);
            objRequest.Method = WebRequestMethods.Http.Get;
            objRequest.Accept = "text/csv";
            objRequest.Headers["Authorization"] = "bearer " + token;
            
            return objRequest;
        }
    }
}
