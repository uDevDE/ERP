using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ERP.Server.Host.Helpers
{
    public static class NRServiceHelper
    {
        public static string Sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public static string GetProjectBasePath() => ConfigurationManager.AppSettings["ProjectBasePath"];

        public static List<string> GetProjectPaths() => ConfigurationManager.AppSettings["ProjectPaths"].Split(',').ToList();


    }
}
