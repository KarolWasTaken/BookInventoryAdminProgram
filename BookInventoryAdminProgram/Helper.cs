using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BookInventoryAdminProgram
{
    public static class Helper
    {

        /// <summary>
        /// Returns connection string for BookstoreInventoryDB
        /// </summary>
        /// <returns></returns>
        public static string? CnnVal()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            Settings? settings = config.GetRequiredSection("Settings").Get<Settings>();
            return settings?.ConnectionString;
        }
    }
}
