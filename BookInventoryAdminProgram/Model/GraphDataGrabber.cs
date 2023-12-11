using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Model
{
    public class GraphDataGrabber
    {
        public enum PopularityGraphProperty
        {
            Author,
            Genre,
            Book
        }

        private class SQLResult
        {
            public int ID { get; set; }
            public int TotalSales { get; set; }
        }


        private Dictionary<string, List<CommonValues>> _junctionValuesDictionary;
        public GraphDataGrabber(Dictionary<string, List<CommonValues>> junctionValuesDictionary)
        {
            _junctionValuesDictionary = junctionValuesDictionary;
        }
        public Dictionary<string, int> GetGraphData(PopularityGraphProperty property)
        {
            List<SQLResult> data;
            string propertyName = property.ToString().ToUpper();
            List<CommonValues> propertyDictionaryPrecursor = _junctionValuesDictionary[property.ToString()];
            Dictionary<int, string> propertyReferenceDictionary = ExtractDictionary(propertyDictionaryPrecursor);
            Dictionary<string, int> salesByNames = new Dictionary<string, int>();


            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                data = connection.Query<SQLResult>("dbo.spGetPopularity @Property", new { Property = propertyName }).ToList();
            }

            foreach (SQLResult element in data)
            {
                int idToMatch = element.ID; // Get the ID from each SQLResult object

                // Check if the ID exists in propertyReferenceDictionary
                if (propertyReferenceDictionary.ContainsKey(idToMatch))
                {
                    string name = propertyReferenceDictionary[idToMatch]; // Get the name linked to the ID

                    // Add the name and TotalSales to the new dictionary
                    if (!salesByNames.ContainsKey(name))
                    {
                        salesByNames[name] = element.TotalSales;
                    }
                    else
                    {
                        // If the name already exists in the dictionary, aggregate the TotalSales
                        salesByNames[name] += element.TotalSales;
                    }
                }
            }
            return salesByNames;
        }
        private static Dictionary<int, string> ExtractDictionary(List<CommonValues> precursor)
        {
            Dictionary<int, string> propertyDict = new Dictionary<int, string>();
            foreach(CommonValues item in precursor)
            {
                propertyDict.Add(item.ID, item.Name);
            }
            return propertyDict;
        }
    }
}
