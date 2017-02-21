using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Domo
{
    class Program
    {
        static void Main(string[] args)
        {
            string datasetID = "*******************************";

            if (args.Length > 0)
            {
                datasetID = args[0];
            }

            DomoDatesetClient.ExportDataset(datasetID);
        }
    }
}
