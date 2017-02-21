using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Domo
{
    public static class AppConfiguration
    {
        public static string UserName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString(); }
        }
        public static string Password
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Password"].ToString(); }
        }            
        public static string BasicToken
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["BasicToken"].ToString(); }
        }
        public static string OutputFilePath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["OutputFilePath"].ToString(); }
        }
    }
}
