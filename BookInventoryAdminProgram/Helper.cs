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

        private static IConfigurationRoot Config
        {
            get {
                // Load the existing JSON file when needed. Ensures up-to-date JSON is always used.
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build(); 
            }
        }

        /// <summary>
        /// Returns settings object with up-to-date settings for BookstoreInventoryDB
        /// </summary>
        /// <returns></returns>
        public static Settings ReturnSettings()
        {
            Settings? settings = Config.GetRequiredSection("Settings").Get<Settings>();
            return settings;
        }


        public static void UpdateMinBookStockNotificationJson(int minBookStockNotification)
        {
            // if I use ReturnSettings(), the header visibility checkbox will need to be pressed twice for it to do anything.
            // but when i do Config.GetRequiredSection("Settings").Get<Settings>().HeaderVisibilitiesSerialised everything works fine
            // This upsets me.
            var updatedConfig = new
            {
                Settings = new
                {
                    ConnectionString = Config["Settings:ConnectionString"],
                    HeaderVisibilitiesSerialised = Config.GetRequiredSection("Settings").Get<Settings>().HeaderVisibilitiesSerialised,
                    LowBookStockWarningCount = minBookStockNotification
                }
            };
            WriteToJson(updatedConfig);
        }


        /// <summary>
        /// Updates appsettings.JSON with new headerVisibility
        /// </summary>
        /// <param name="newDict"></param>
        public static void UpdateHeaderVisibilityJson(Dictionary<string, bool> newDict)
        {
            // Create a new JSON structure with the Settings object (to preserve structure)
            var updatedConfig = new
            {
                Settings = new
                {
                    ConnectionString = Config["Settings:ConnectionString"],
                    HeaderVisibilitiesSerialised = newDict,
                    LowBookStockWarningCount = Config["Settings:LowBookStockWarningCount"]
                }
            };
            WriteToJson(updatedConfig);
        }

        private static void WriteToJson(dynamic jsonConfig)
        {
            // turn object into json string for filewriting
            string updatedJson = JsonConvert.SerializeObject(jsonConfig, Formatting.Indented);
            // Write to file
            File.WriteAllText("appsettings.json", updatedJson);

        }
    }
}
