using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Stores
{
    /// <summary>
    /// class made for storing the main dataset for our program used in the inventory and other places
    /// </summary>
    public class MainDatasetStore
    {
        public string ISBN { get; set; }
        public int BookID { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Genres { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PublisherName { get; set; }
        public int AllTimeSales { get; set; }
        public double AllTimeRevenue { get; set; }
        public int YearlySales { get; set; }
        public double YearlyRevenue { get; set; }
        public int MonthlySales { get; set; }
        public double MonthlyRevenue { get; set; }
        public int DailySales { get; set; }
        public double DailyRevenue { get; set; }
        
    }
}
