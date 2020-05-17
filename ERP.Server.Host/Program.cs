using ERP.Contracts.Contract;
using ERP.Server.Host.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Server.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfiguration.Configure();
            NRService.Configure();

            Console.WindowWidth = 90;

            var tagFilename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tag.txt");
            if (System.IO.File.Exists(tagFilename))
                Console.WriteLine(System.IO.File.ReadAllText(tagFilename));

            var uris = new Uri[1];
#if DEBUG
            string addr = "net.tcp://localhost:6565/NRService";
#else
            string addr = "net.tcp://167.86.73.240:6565/NRService";
#endif
            uris[0] = new Uri(addr);
            INRService service = new NRService();
            ServiceHost host = new ServiceHost(service, uris);
            var binding = new NetTcpBinding("TcpBinding");
            host.AddServiceEndpoint(typeof(INRService), binding, string.Empty);
            host.Opened += Host_Opened;
            host.Open();

            Console.ReadLine();
            host.Close();
        }

        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Tcp service started");
        }

    }
}
