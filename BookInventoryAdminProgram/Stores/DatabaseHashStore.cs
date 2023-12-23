using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Stores
{
    public class DatabaseHashStore
    {
        private string oldHash { get; set; }
        public string OldHash
        {
            get { return oldHash; }
        }

        private string newHash { get; set; }
        public string NewHash
        {
            get { return newHash; }
        }
        public void UpdateHash(string hash)
        {
            if (oldHash == null)
            {
                oldHash = hash;
            }
            else if (newHash == null && oldHash != null)
            {
                newHash = hash;
            }
            else
            {
                oldHash = newHash;
                newHash = hash;
            }
        }

    }
}
