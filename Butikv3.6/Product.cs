using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class Product
    {
        public int price;
        public string name;
        public string type;
        public string summary;
        public string imageLocation;
        public int nrOfProducts;

        /// <summary>
        /// Returns a string representation of the Product object in CSV-file format.
        /// </summary>
        public string ToCSV()
        {
            return $"{price},{name},{type},{summary},{imageLocation},{nrOfProducts}";
        }
    }
}
