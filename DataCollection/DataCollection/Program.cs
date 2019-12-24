using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            OPCHelp oPCHelp = new OPCHelp();
            string localName = oPCHelp.GetLocalName();
            Console.WriteLine(localName);

            List<string> ip = oPCHelp.GetLocalIP();
            for (int i = 0; i < ip.Count; i++)
            {
                Console.WriteLine(ip[i]);
            }

            List<string> name= oPCHelp.GetOpcServerName(localName);
            for (int i = 0; i < name.Count; i++)
            {
                Console.WriteLine(name[i]);
            }

            Console.ReadKey();
        }
    }
}
