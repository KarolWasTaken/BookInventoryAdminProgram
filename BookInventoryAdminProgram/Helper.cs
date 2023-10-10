using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace BookInventoryAdminProgram
{
    public static class Helper
    {
        /// <summary>
        /// Returns settings object with up-to-date settings for BookstoreInventoryDB
        /// </summary>
        /// <returns></returns>
        public static Settings ReturnSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            Settings? settings = config.GetRequiredSection("Settings").Get<Settings>();
            return settings;
        }

        /// <summary>
        /// Updates appsettings.JSON with new headerVisibility
        /// </summary>
        /// <param name="newDict"></param>
        public static void UpdateHeaderVisibilityJson( Dictionary<string, bool> newDict)
        {
            // Load the existing JSON file
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Create a new JSON structure with the Settings object (to preserve structure)
            var updatedConfig = new
            {
                Settings = new
                {
                    ConnectionString = config["Settings:ConnectionString"],
                    HeaderVisibilitiesSerialised = newDict
                }
            };

            // turn object into json string for filewriting
            string updatedJson = JsonConvert.SerializeObject(updatedConfig, Formatting.Indented);

            // Write to file
            File.WriteAllText("appsettings.json", updatedJson);
        }
    }
}
