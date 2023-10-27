using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public class SalesFeatures
    {


        public enum ReportType
        {
            Revenue,
            QuantitySold
        }
        public enum SalesQuarters
        {
            First,
            Second,
            Third,
            Fourth
        }


        public SalesFeatures()
        {

        }
    }
}
