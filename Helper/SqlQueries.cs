using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegistration.Helper
{
    public class SqlQueries
    {
              
            static IConfiguration queriesConfig = new ConfigurationBuilder()
                .AddXmlFile("SqlQueries.xml", true, true)
                .Build();

            public static string RegisterEmployee { get { return queriesConfig["RegisterEmployee"]; } }
        public static string LoginEmployee { get { return queriesConfig["LoginEmployee"]; } }
        public static string GetAdmin { get { return queriesConfig["GetAdmin"]; } }
        public static string GetUser { get { return queriesConfig["GetUser"]; } }


    }
}
